# Diagnostic Constraint Language

Diagnostic Constraint Language (DCL) is domain specific language to write rules in form of constraints that 
idnetifies malformed, incorrect or otherwise damaged packet communication. The DCL uses simple syntax based 
on YAML format and Wireshark display expressions. The evaluation of the SCL rules can be done efficiently 
by executing SQL like queries. A problematic communication can be also identified as the evaluaiton of rules 
usually yields to the collection of events.

## Expressions
DCL expressions are based on Wireshark display filter expressions. Expressions are composed of the following terms:
* Fields 
* Comparison operators
* Constants
* Logical operators

The syntax of expressions in ABNF is:
```
term          = field / constant 

comparison_op = "==" / "!=" / ">" / "<" / "<=" / ">=" / "contains" / "~" / "&"

expression    = term comparison_op term

logical_op    = "&&" / "||" / "^^" / "in" 

expression    = "!" expression
              / expression logical_op expression
              / ( expression )           
```
### Fields
Every field provided by the protocol parser can be used in the expression. Field reference can contain fully qualified field name, for instance ```tcp.port```.

### Comparison Operators
Expression can be build using a number of different comparison operators. 

| Operator       | Description  |
| -------------- | ------------ |
| `==`           | Equal        |
| `!=`           | Not equal    |
| `>`            | Greater than |
| `<`            | Less than        |
| `>=`           | Less than or equal | 
| `<=`           | Greater than or equal |
| `contains`     | Protocol, field or slice contains a value. |
| `&`            | Compare bit field value, e.g. `tcp.flags & 0x02` |

### Constants
Fields have different types. The supported types are:
* Unsigned integer - decimal (```1*DIGIT```), octal (```"0" 1*DIGIT```), or hexadecimal (```"0x" 1*HEXDIGIT```) formats are possible. 
* Signed Integer - decimal (```["-"] 1*DIGIT```), octal (```["-"] "0" 1*DIGIT```), or hexadecimal (```["-"] "0x" 1*HEXDIGIT```) formats are possible.   
* Boolean - any of the following is a valid boolean constant ```"TRUE", "FALSE", "true", "false", "0", "1"```.
* Ethernet Address - three syntax forms are possible: ```HEXDIGIT HEXDIGIT 5(":" HEXDIGIT HEXDIGIT)```, 
```HEXDIGIT HEXDIGIT 5("-" HEXDIGIT HEXDIGIT)```, or ```HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT 2("." HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT)```.
* IPv4 Address - TODO
* IPv6 Address - TODO
* Text String - TODO (usual C string enclosed in ")

### Logical Operators

| Operator       | Description  |
| -------------- | ------------ |
| `&&`       | logical AND  |
| `||`       | logical OR   |
| `^^`       | logical XOR  |
| `!`        | logical NOT  |
| `in`       | Membership operator, e.g. ```tcp.port in {80 443 8080}``` |


## Constraints
Constraints are built from expressions and temporal operators. The syntax of constraints in ABNF is:
```
temporal_op     = "~>" 
                / "{" event_range "}~>"
                / "[" time_range "]~>"

event_expression = expression

constraint      = expression
                / event_expression temporal_op event_expression
```
### Event Expression
Event expresssion is an expression that when evaluated gives a set of events that satisfies the given expression. 
It is possible that no event satisfies the expression and in this case this set is empty.

### Temporal Operators
Constraints can also be composed using temporal operators on event expressions.
A single temporal operator of the language is *leads to* operator:

* `A ~> B` is defined as `[](A => <>B)`.
Where `A` and `B` are flow expressions.

It is possible to annotate *leads to* operator with either event interval or time range:

* Event interval ```A {X..Y}~> B```, where X and Y are positive integer numbers. If X = Y it is possible to write ```A {X}~> B```.

* Time range ```A [X-Y]~> B```, where X and Y are positive numbers that can have associated units of measure. Possible units are ```us```, ```ms```, ```s```, ```m```, ```h```, ```d```. It is be possible to write values such as ```1d2h30m15s```.

## Rules
A rule is a collection of event expressions `E1,...,En`, collection of constraints `C1,...,Cm`, 
and result selectors `R1,...,Rk`. We use YAML syntax to specify rules:

```yaml
events:
    e1: E1
        ...
    en: En
assert:
    - C1
    ...
    - Cm
select:
    r1: R1
    ...
    rk: Rk
```
Rule consists of three blocks. 

* Events are defined as maps because each event is assigned a name and expression that provides the event set selector. 
* Assert block consists of a list of constraints. 
* Select block constains map for creating resulting structure.

## Model and Interpretation
To evaluate rule in the given data the event model is defined. Data source is 
organized as the collection of events. Evaluating rule is then performed by translating
the rule into LINQ query and executing this query for the provided input collection fo events.

### Events
The model is based on events. An event is defined as:

```protobuf
message Event 
{
    uint32 ts_sec = 1;
    uint32 ts_usec = 1;
    bytes flow = 2;
    string origin = 3;
    int32 eid = 5;
    map<string.string> attributes = 5;
}
```
Fields of Event structure have the following meaning:
* ts_sec - POSIX timestamp of the event
* ts_usec - microseconds to add to the POSIX timestamp (https://www.elvidence.com.au/understanding-time-stamps-in-packet-capture-data-pcap-files/)
* flow - identification of the flow of the event
* origin - source module that produces the event (protocol analyzer)
* eid - unique id of the event with respect to the flow
* attributes - dictionary of event attributes, usually, these correspond to the protocol field and its value.

### Expressions
Evaluating an expression against the event set results in a collection of events that matches the given expression.
The language is encoded using YAML (to test the correctness of the syntax see tool at: http://www.yamllint.com/ or 
http://yaml-online-parser.appspot.com/)

For instance, 
```yaml
---
events:
    e: pop.request.command == ’AUTH’
```
yields to all events that represent authentication command in a POP session. The corresponding 
LINQ is created as follows:
```cs
from e in events 
where e.Satisfy("pop.request.command == 'AUTH'") 
select new { e };
```

An example of the rule that identifies failed POP authentication is:
```yaml
---
events:
    e1: pop.request.command == 'AUTH'
    e2: pop.response.indicator == '-ERR'
assert:
    - e1.flow == e2.flow
    - e1 {1}~> e2
```

Matching events using a specified attribute(s) can be realized by inner join in LINQ:
```cs
from e1 in events 
join e2 in events on e1.flow equals e2.flow
where e1.Satisfy("pop.request.command == 'AUTH'") && e2.Satisfy("pop.response.indicator == '-ERR'")
   && e1.eid+1 == e2.eid
select new {e1,e2};
```
The next example shows detection of errorneous DNS resolution and customized output:
```yaml
events:
    e1: dns.flags.response==0 
    e2: dns.flags.response==1 && dns.flags.rcode!=0
assert: 
    - e1.dns.id = e2.dns.id
    - e1 [30s]~> e2
select:
    query: e1
    answer: e2
    reason: "DNS error"
```
The ```select``` attribute can be used to create specific result instead of the default 
output. The LINQ generated uses anonymous type for the result:

```cs
from e1 in events 
join e2 in events on e1["dns.id"] equals e2["dns.id"]
where e1.Satisfy("dns.flags.response==0") 
   && e2.Satisfy("dns.flags.response==1 && dns.id==id && dns.flags.rcode!=0")
   && e1.ts_sec >= e2.ts_sec && e2.ts_sec <= e1.ts_sec + 30  
select new {query = e1, answer = e2, reason = "DNS error"};
```

## Detecting Absence of Event

## Composing rules


# References
* Wireshark - Building display filter expressions
https://www.wireshark.org/docs/wsug_html_chunked/ChWorkBuildDisplayFilterSection.html
* Wireshark - Display Filter Reference
https://www.wireshark.org/docs/dfref/
* Overview of the Event Processing Language (EPL)
https://docs.oracle.com/cd/E13157_01/wlevs/docs30/epl_guide/overview.html
* YAML 1.2
http://yaml.org/
* YAML @ Microsoft
https://docs.microsoft.com/en-us/contribute/ops-crr/openpublishing/docs/partnerdocs/yaml

