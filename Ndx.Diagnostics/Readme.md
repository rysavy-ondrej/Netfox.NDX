


# Example
Following rules performs a correlation of two basic events found in the source packet capture.

```yaml
rule:
    id: dns_query_response_ok
    description: Correlates sucessful DNS query and response into a single event. 
    result: (query: e1, reply: e2)
params:
    - dnsClient
events:
    e1: dns.flags.response == 0
    e2: dns.flags.response == 1 && dns.flags.rcode == 0
assert:    
    - dnsClient.ip.adr == e1.ip.src
    - e1.ip.src eq '192.168.111.100'
    - e1.dns.id == e2.dns.id
    - e1.timestamp < e2.timestamp && e2.timestamp <= e1.timestamp + 2000 
```