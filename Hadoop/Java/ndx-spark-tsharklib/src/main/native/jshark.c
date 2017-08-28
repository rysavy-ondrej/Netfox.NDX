// g++ t.cpp -I/usr/include/glib-2.0 -I/usr/lib/glib-2.0/include -I. -I../include/ -lpthread -L/home/santosh/proj -lpcap -L/home/santosh/proj  -lwireshark

#include "timestamp.h"
#include "prefs.h"

void initialize_epan(void)
{
  int i;
  e_prefs *prefs;
  char *gpf_path, *pf_path;
  int gpf_open_errno, gpf_read_errno;
  int pf_open_errno, pf_read_errno;

  //set timestamp type
  timestamp_set_type(TS_RELATIVE);

  // This function is called when the program starts, to save whatever credential information
  // we'll need later, and to do other specialized platform-dependent initialization
  init_process_policies();
  
  epan_init(register_all_protocols, register_all_protocol_handoffs,
    NULL, NULL, failure_message, open_failure_message,
    read_failure_message, NULL);
  
  
  // Register all non-dissector modules' preferences.
  prefs_register_modules();

  // Read the preferences file, fill in "prefs", and return a pointer to it, 
  // preference file has information about protocol preferences (e.g. default port)
  prefs = read_prefs(&gpf_open_errno, &gpf_read_errno, &gpf_path,
    &pf_open_errno, &pf_read_errno, &pf_path);
  
  if (gpf_path != NULL) {
    if (gpf_open_errno != 0)
      fprintf(stderr, "Can't open global preferences file \"%s\": %s.\n", pf_path, strerror(gpf_open_errno) );
    
    if (gpf_read_errno != 0)
      fprintf(stderr, "I/O error reading global preferences file " "\"%s\": %s.\n", pf_path, strerror(gpf_read_errno) );
  }

  if (pf_path != NULL) {
    if (pf_open_errno != 0)
      fprintf(stderr, "Can't open your preferences file \"%s\": %s.\n",pf_path, strerror(pf_open_errno));
    
    if (pf_read_errno != 0)
      fprintf(stderr, "I/O error reading your preferences file " "\"%s\": %s.\n", pf_path, strerror(pf_read_errno));
    
    g_free(pf_path);
    pf_path = NULL;

  }

  cleanup_dissection();

  // Initialize the dissection engine
  init_dissection();

  /* Set the given nstime_t to (0,maxint) to mark it as "unset"
   * That way we can find the first frame even when a timestamp
   * is zero */

  nstime_set_unset(&first_ts);
  nstime_set_unset(&prev_cap_ts);
}

void failure_message(const char *msg_format, va_list ap)
{
  vfprintf(stderr, msg_format, ap);
  fprintf(stderr, "\n");
}

void open_failure_message(const char *filename, int err, gboolean for_writing)
{
  fprintf(stderr, "open error. filename = %s, err = %d, for_writing = %d\n",
    filename, err, for_writing);
}


void fill_framedata(frame_data *fdata, uint64_t frame_number,
    const struct pcap_pkthdr *h, int ll_type)
{
    fdata->pfd = NULL;
    fdata->num = frame_number;
    fdata->pkt_len = h->len;
    fdata->cum_bytes  = 0;
    fdata->cap_len = h->caplen;
    fdata->file_off = 0;
    fdata->lnk_t = ll_type;
    fdata->abs_ts.secs = h->ts.tv_sec;
    fdata->abs_ts.nsecs = h->ts.tv_usec*1000;
    fdata->flags.passed_dfilter = 0;
    fdata->flags.encoding = CHAR_ASCII;
    fdata->flags.visited = 0;
    fdata->flags.marked = 0;
    fdata->flags.ref_time = 0;
    fdata->color_filter = NULL;

    if (nstime_is_unset(&first_ts) )
    first_ts = fdata->abs_ts;

    nstime_delta(&fdata->rel_ts, &fdata->abs_ts, &first_ts);

    if (nstime_is_unset(&prev_cap_ts) )
    prev_cap_ts = fdata->abs_ts;

    nstime_delta(&fdata->del_cap_ts, &fdata->abs_ts, &prev_cap_ts);
    fdata->del_dis_ts = fdata->del_cap_ts;
    prev_cap_ts = fdata->abs_ts;
}



void clear_fdata(frame_data *fdata)
{
    if (fdata->pfd) g_slist_free(fdata->pfd);
}

void process_packet(u_char *user, const struct pcap_pkthdr *h, const u_char *bytes)
{
    (void) user;
    
    // declare dissection tree data structure, it will contain all the packet information (all the layers)
    epan_dissect_t *edt;

    //declare the frame_data strcture that will be used in populating frame data
    frame_data fdata;
    
    //pseaudo header 
    union wtap_pseudo_header pseudo_header;
    
    static uint32_t frame_number; /* Incremented each time libpcap gives us a packet */
    
    memset(&pseudo_header, 0, sizeof(pseudo_header) );
    
    frame_number++;
    
    fill_framedata(&fdata, frame_number, h, ll_type);
    
    // get new dissection tree 
    edt = epan_dissect_new(verbose /* create_proto_tree */,
                            verbose /* proto_tree_visible */);
    
    // execute dissection engine on frame data
    epan_dissect_run(edt, &pseudo_header, bytes, &fdata,
                    !verbose ? &cinfo : NULL);
    if (verbose)
        proto_tree_print(edt); //print the packet information

    //free the dissection tree   
    epan_dissect_free(edt);

    // free the frame data 
    clear_fdata(&fdata);
}

