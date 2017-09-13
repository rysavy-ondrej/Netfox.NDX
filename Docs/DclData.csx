var events = new IDictionary<string,string> []
{
    new Dictionary<string, string>  { {"eid", "1"}, { "dns.id", "1234" }, { "dns.flags.response", "0" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "2"}, { "dns.id", "1234" }, { "dns.flags.response", "1" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "3"}, { "dns.id", "2564" }, { "dns.flags.response", "0" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "4"}, { "dns.id", "8819" }, { "dns.flags.response", "0" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "5"}, { "dns.id", "4271" }, { "dns.flags.response", "0" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "6"}, { "dns.id", "2564" }, { "dns.flags.response", "1" }, {"dns.flags.rcode","0"} },
    new Dictionary<string, string>  { {"eid", "6"}, { "dns.id", "4271" }, { "dns.flags.response", "1" }, {"dns.flags.rcode","0"} },   
};

var res = 
from e1 in events.Where(e => e["dns.flags.response"].Equals("0")) 
join e2 in events.Where(e => e["dns.flags.response"].Equals("1") && !e["dns.flags.rcode"].Equals("0"))  
  on e1["dns.id"] equals e2["dns.id"]
where Int32.Parse(e1["eid"]) < Int32.Parse(e2["eid"])
select new {query = e1, answer = e2, reason = "DNS error"};




var t = 
from e1 in events.Where(e => e["dns.flags.response"].Equals("0"))
join e2 in events.Where(e => e["dns.flags.response"].Equals("1"))
on e1["dns.id"] equals e2["dns.id"] into grp
from right in grp.DefaultIfEmpty()
where right == null
select (new { e1 = e1, desc =  "No reply" });