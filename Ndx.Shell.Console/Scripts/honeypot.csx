// C# Script
// Press Alt+Enter to evaluate a single line or selection
// Press Alt+Shift+Enter to evaluate the whole script


var files = Directory.EnumerateFiles(@"P:\Data\pcap");

var frames = Capture.Filter(Filter.Address("192.168.186.47"), files.ToArray());

var outfile = new CaptureFileWriterDevice(@"P:\Data\hosts\192.168.186.47.cap");

foreach(var frame in frames)
{
	Capture.Write(outfile, frame);
}