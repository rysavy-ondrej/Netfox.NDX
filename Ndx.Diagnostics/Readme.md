


# Example

## Rules
Following rules performs a correlation of two basic events found in the source packet capture.

```yaml
rule:
    name: dns_resolution
    description: Correlates DNS query and response into a single event.
    result: DNS_QUERY_RESPONSE(query: f1, response: f2)
params:
    - flow
flows:
    f1: dns.flags.response == 0
    f2: dns.flags.response == 1 && dns.flags.rcode == 0
assert:    
    - flow.ip.src == f1.ip.src
    - f1.ip.src eq '192.168.111.100'
    - f1.dns.id == f2.dns.id
    - f1.timestamp < f2.timestamp && f2.timestamp <= f1.timestamp + 2000 
```


Parameters and decoded packets can represented as YAML structure. Thus when evaluating `assert` the following data structure 
is passed to assert predicate:
```yaml
f1:
    timestamp: 1084443427311
    frame:
        number: 1
        protocols: 'eth:ethertype:ip'
    eth:
        dst: 'fe:ff:20:00:01:00'
        src: '00:00:01:00:00:00'
        type: 0x00000800 
    ip: 
        proto: 6
        ttl: 128
        src: '145.254.160.237'
        dst: '65.208.228.223'
        id: 0x00000f41
        len: 48
    udp: ...
    dns: ...
f2: ...

flow:
    ip:
        src: '65.208.228.223'
```

This rule emits events composed from all pairs of f1, f2 that pass the predicate `assert`. This is represented as 
event structure, e.g.:

```yaml
event: 
    name: DNS_QUERY_RESPONSE
    items:
        query: 
            timestamp: 1084443427311
            frame:
                number: 1
                protocols: 'eth:ethertype:ip'
            eth:
                dst: 'fe:ff:20:00:01:00'
                src: '00:00:01:00:00:00'
                type: 0x00000800 
            ip: 
                proto: 6
                ttl: 128
                src: '145.254.160.237'
                dst: '65.208.228.223'
                id: 0x00000f41
                len: 48 
            udp: ...
            dns: ...
        reply: 
            timestamp: 1084443427311
            frame:
                number: 1
                protocols: 'eth:ethertype:ip'
            eth:
                dst: '00:00:01:00:00:00'
                src: 'fe:ff:20:00:01:00'
                type: 0x00000800 
            ip: 
                proto: 6
                ttl: 128
                src: '65.208.228.223'
                dst: '145.254.160.237'
                id: 0x00000f41
                len: 48
            upd: ...
            dns: ...
```
# YAML to Python
YAML structure can be easily represented in Python. For example the input to assert predicate is following:  
```python
{'f1': {'dns': '...',
        'eth': {'dst': 'fe:ff:20:00:01:00',
                'src': '00:00:01:00:00:00',
                'type': 2048},
        'frame': {'number': 1, 'protocols': 'eth:ethertype:ip'},
        'ip': {'dst': '65.208.228.223',
               'id': 3905,
               'len': 48,
               'proto': 6,
               'src': '145.254.160.237',
               'ttl': 128},
        'timestamp': 1084443427311L,
        'udp': '...'},
 'f2': '...',
 'flow': {'ip': {'src': '65.208.228.223'}}}
```


## Event Tree
```yaml
tree:
    id: dns_check
params:
    - dnsFlow
root:
    info: Dns analysis started.
    event: dns_query_exists(dnsFlow)
    none:
        info: Dns query not found.
        emit: dns_not_found(dnsFlow)
    some:
        match: e
        info: Dns query found.        
        event: dns_reply_exists(e)
        none:            
            emit: dns_no_reply(e)
        some: 
            match: (e1,e2)
            event: dns_reply_ok(e1,e2)
            none: 
                emit: dns_reply_err(e1,e2)
            some: 
                match: (e1,e2)
                emit: dns_reply_ok(e1,e2)
```


