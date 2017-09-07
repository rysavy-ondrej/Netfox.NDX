# PShark Component
This components provides application packet parsers. It is based on the two 
existing libraries:

* Kaitai Struct library for parsing binary protocols - http://doc.kaitai.io/user_guide.html

* A library for parsing text protocols specified in ABNF (https://tools.ietf.org/html/rfc5234)
    * aParser library (http://www.parse2.com/index.shtml)
    * Grammatica library (https://grammatica.percederberg.net/index.html)
    * Coco/R (http://www.ssw.uni-linz.ac.at/Research/Projects/Coco/#CS)

* Extracting ABNF from RFC: https://tools.ietf.org/abnf/

Parsing input packets depends on the protocol. Text protocols usualy deals with data streams and 
buffering can be done by per line basis. Binary protocols rather than that relies on the fixed or structured
data. See https://www.bro.org/sphinx/components/binpac/README.html#parsing-methodology.
It means that the parser should advise the data provider on how to offer data for parsing. 
The different data input can be:

* Text Stream
* Binary Stream
* Buffer
* Other?



# Binary protocols


## DNS

## SMB

##

# Text Protocols


## HTTP

## SMTP

# Resources
Some other resources related to Network Protocol PArsing:
* https://curve.carleton.ca/system/files/etd/1fbf53a2-9b3f-4dd6-826e-ad91af12116d/etd_pdf/595d50e82abe7fb03a34d3eb139814da/hughes-parsingstreamingnetworkprotocols.pdf