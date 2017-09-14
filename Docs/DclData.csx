
var events = TSharkDataset.LoadEventsFromFile(@"C:\Users\Ondrej\Downloads\mail-pcholkovic.pcap.smtp");

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




