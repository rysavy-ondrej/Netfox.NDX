# Diagnostic Constraint Language

Diagnostic Constraint Language (DCL) is domain specific language to write rules in the form of constraints that identifies malformed, 
incorrect or otherwise damaged packet communication. The DCL uses simple syntax based on YAML format and Wireshark display expressions. 
The evaluation of the SCL rules can be done efficiently by executing SQL like queries. A problematic communication can also be identified 
as the evaluation of rules usually, yields to the collection of events.

## Expressions
The DCL expressions are based on Wireshark display filter expressions. Expressions are composed of the following terms:
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
Every field provided by the protocol parser can be used in the expression. Field reference can contain fully qualified field name, for instance, ```tcp.port```.

| Priority | Associaton | Operator               | Description  |
| -------- | ---------- | ---------------------- | ------------ |
| 1        | Left       | BlockName`.`MemberName | Access members in a block.  

### Constants
Fields have different types. The supported basic types are:

| Type    | Description       |
| ------- | ----------------- |
| bool    |	   |
| string  |	   |
| bytes   |	   |
| int32	  |	   |
| uint32  |	   |
| int64   |	   |
| uint64  |	   |
| float   |	   |

In addition, it is possible to use the following constants:
* Ethernet Address - three syntax forms are possible: ```HEXDIGIT HEXDIGIT 5(":" HEXDIGIT HEXDIGIT)```, 
```HEXDIGIT HEXDIGIT 5("-" HEXDIGIT HEXDIGIT)```, or ```HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT 2("." HEXDIGIT HEXDIGIT HEXDIGIT HEXDIGIT)```.
Comverted to bytes.
* IPv4 Address - Represented as bytes.
* IPv6 Address - Represented as bytes.

### Comparison Operators
The expression can be build using the following comparison operators. 

| Priority | Associaton | Operator       | Description  |
| -------- | ---------- | -------------- | ------------ |
| 6        | Left       | Operand `>` Operand   | Greater than |
| 6        | Left       | Operand `<` Operand   | Less than    |
| 6        | Left       | Operand `>=` Operand  | Less than or equal | 
| 6        | Left       | Operand `<=` Operand  | Greater than or equal |
| 7        | Left       | Operand `==` Operand  | Equal        |
| 7        | Left       | Operand `!=` Operand  | Not equal    |
| 7        | Left       | Operand `<>` Operand	| Not equal    |

### Set Operators

| Priority | Associaton | Operator       | Description  |
| -------- | ---------- | -------------- | ------------ |
| ? | Left | Operand `contains` Value    | Protocol, field or slice contains a value. |
| ? | Left | Operand `in` Set            | Membership operator, e.g. ```tcp.port in {80 443 8080}``` |

### Bits Operators

| Priority | Associaton | Operator       | Description  |
| -------- | ---------- | -------------- | ------------ |
| 2        | Right      | `~` Operand            | Bits Complement     |
| 5        | Left       | Operand `<<` Number           | Bits Left Shift |
| 5        | Left       | Operand `>>` Number           | Bits Right Shift |
| 8        | Left       | Operand `&` Operand          | Bits AND     |
| 9        | Left       | Operand `^` Operand			 | Bits XOR     |
| 10       | Left       | Operand `|` Operand			 | Bits OR      |


### Logical Operators

| Priority | Associaton | Operator       | Description  |
| -------- | ---------- | -------------- | ------------ |
| 2        | Right      | `!` Operand               | logical NOT  |
| 11       | Left       | Operand `&&` Operand      | logical AND  |
| 11       | Left       | Operand `AND` Operand     | logical AND  |
| 12       | Left       | Operand `⎜⎜` Operand      | logical OR   |
| 12       | Left       | Operand `OR` Operand      | logical OR   |

### Arithmetic Operators

| Priority | Associaton | Operator       | Description  |
| -------- | ---------- | -------------- | ------------ |
| 2        | Right      | Operand `-` Operand           | Minus        |
| 3        | Left       | Operand `*` Operand			 | Multiply     |
| 3        | Left       | Operand `/` Operand            | Divide       |
| 3        | Left       | Operand `%` Operand            | Remainder    |
| 4        | Left       | Operand `+` Operand            | Add       |
| 4        | Left       | Operand `-` Operand            | Subtract    |


### Concept of NONE
If a protocol does not define a field, the value of the nonexistent field defaults to NONE.
This is more suitable for most of the situations that using some default value if the field is non existent.
For instance the expression:
```
ip.len < 60
```
will yield to false if the frame does not contain ip packet. Using default values this would be evaluated to true. 

## Rules
A rule is a collection of event expressions `E1,...,En`, collection of constraints `C1,...,Cm`, 
and the result selectors `R1,...,Rk`. The YAML syntax is employed to specify rules:

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

* Events are defined as maps because each event is assigned a name and expression that provides the event stream selector. 
* Assert block consists of a list of constraints. The meaning of assert is defined by the conjunction of its constrain expressions.
* Select block contains a map for creating resulting structure.

## Model and Interpretation
Rules are evaluated according to the specified event model. Event data model is a collection of events. Evaluating rule is performed by translating
the rule into LINQ query and executing this query for the provided input collection of events.

### Events
The model is based on events. An event is defined as:

```protobuf
message Event 
{
    int64 timestamp = 1;    
    bytes flow = 2;
    string origin = 3;
    int32 eid = 4;
    repeated int32 packets = 5;
    map<string,string> attributes = 8;
}
```
Fields of Event structure have the following meaning:
* timestamp - the timestamp value encoded the number of miliseconds since UNIX Epoch.
* flow - identification of the flow of the event
* origin - source module that produces the event (protocol analyzer)
* eid - unique id of the event with respect to the flow
* packets - a collection of packet numbers for referencing the source of the event
* attributes - dictionary of event attributes, usually, these correspond to the protocol fields and their values.

### Expressions
Evaluating an expression against the event set results in a collection of events that matches the given expression.

For instance, 
```yaml
---
events:
    e: pop.request.command == ’AUTH’
```
yields to all events that represent authentication command in a POP session. The corresponding 
LINQ is created as follows:
```cs
from e in events.Where(e => e.Satisfy("pop.request.command == 'AUTH'")) 
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
We use ` e1 {1}~> e2` constraint to match the request with the immediate response. 

The next example shows the detection of DNS resolution that ends with an error. 
```yaml
---
events:
    e1: dns.flags.response==0 
    e2: dns.flags.response==1 && dns.flags.rcode!=0
assert: 
    - e1.dns.id == e2.dns.id
    - e1 [0-30s]~> e2
select:
    query: e1
    answer: e2
    reason: "DNS error replied."
```
The ```select``` attribute can be used to create a custom result instead of the default 
output. 

## Detecting Absence of Event
For instance, we would like to 
create rule that expresses the situation when no reply to DNS request is 
received. It is stated as `e1 ~> !e2` that express the situation that there is `e1`
which is not followed by `e2`.

```yaml
---
events:
    e1: dns.flags.response==0 
    e2: dns.flags.response==1    
assert:
    - e1.dns.id == e2.dns.id
    - e1 ~!> e2
select:
    query: e1   
    desc: "DNS no reply."
```
Note that referencing `e2` in `select` is possible and it gives undefined event.

## Parametrized Rules

```yaml
params:
    - host
events:
    e1: dns.flags.response==0 && ip.src == host
    e2: dns.flags.response==1    
assert:
    - e1.dns.id == e2.dns.id
    - e1 ~!> e2
select:
    query: e1   
    desc: "DNS no reply."
``` 

## Named rules

```yaml
rule:
    id: dns_no_reply 
params:
    - host
events:
    e1: dns.flags.response==0 && ip.src == host
    e2: dns.flags.response==1    
assert:
    - e1.dns.id == e2.dns.id
    - e1 ~!> e2
select:
    query: e1   
    desc: "DNS no reply."
``` 


## Composing rules

```yaml
rule:
    id: dns_check
params:
    - host
select:
    result: dns_no_reply(host: host) | dns_error(host: host) | dns_ok(host: host)
```


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
* YAML online parser
http://yaml-online-parser.appspot.com/
* Temporal Operators
https://pdfs.semanticscholar.org/6ed6/404cc710511c2a77d190ff10f83e46324d91.pdf
* Google Protocol Buffers
https://developers.google.com/protocol-buffers/
* LINQ Join Clause
https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause
