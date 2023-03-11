// Licensed under GPL License

using GLib;
using Gst;
using Gst.Video;
using System;
using System.IO;

namespace Streamer
{
    public class PlayerClass
    {

        public PlayerClass()
        {

            try
            {

                Gst.Application.Init();



            }
            catch (Exception ex)
            {

            }
        }
        private Gst.Element LivePlaybin;
        public bool GetLiveVideoStream(System.Windows.Forms.Panel myWindow, string url)
        {
            try
            {


                var MainLoop = new GLib.MainLoop();
                var m_GlibThread = new System.Threading.Thread(MainLoop.Run);

                // Create the elements
                if (LivePlaybin != null)
                {
                    LivePlaybin.SetState(Gst.State.Null);
                }
                else
                {
                    LivePlaybin = Gst.ElementFactory.Make("playbin", "playbin");
                }

             
                LivePlaybin["uri"] = url;
               
                var videosink = Gst.ElementFactory.Make("autovideosink", "autovideosink");
                LivePlaybin["video-sink"] = videosink;
                var bus = LivePlaybin.Bus;
                bus.EnableSyncMessageEmission();
                //bus.SyncMessage += Bus_SyncMessage;
                bus.AddSignalWatch();
                bus.Connect("message::error", HandleError);
                bus.Connect("message::eos", HandleEos);
                bus.Connect("message::state-changed", HandleStateChanged);
                bus.Connect("message::application", HandleApplication);

                // Start playing
                var ret = LivePlaybin.SetState(Gst.State.Playing);
                Gst.Element overlay = null;
                if (LivePlaybin is Gst.Bin)
                    overlay = ((Gst.Bin)LivePlaybin).GetByInterface(VideoOverlayAdapter.GType);

                VideoOverlayAdapter adapter = new VideoOverlayAdapter(overlay.Handle);
                adapter.WindowHandle = myWindow.Handle;
                adapter.HandleEvents(true);
                m_GlibThread.Start();

                if (ret == Gst.StateChangeReturn.Failure)
                {
                    LivePlaybin.SetState(Gst.State.Null);
                    return false;
                }

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        // This function is called when an error message is posted on the bus
        public void HandleError(object sender, GLib.SignalArgs args)
        {
            var msg = (Gst.Message)args.Args[0];
            string debug;
            GLib.GException exc;
            msg.ParseError(out exc, out debug);
            Console.WriteLine(string.Format("Error received from element {0}: {1}", msg.Src.Name, exc.Message));
            Console.WriteLine("Debugging information: {0}", debug);
            // Set the pipeline to READY (which stops playback)
            LivePlaybin.SetState(Gst.State.Ready);
        }

        // This function is called when an End-Of-Stream message is posted on the bus. We just set the pipelien to READY (which stops playback)
        void HandleEos(object sender, GLib.SignalArgs args)
        {
            Console.WriteLine("End-Of-Stream reached.");
            LivePlaybin.SetState(Gst.State.Ready);
        }

        // This function is called when the pipeline changes states. We use it to keep track of the current state.
        void HandleStateChanged(object sender, GLib.SignalArgs args)
        {
            var msg = (Gst.Message)args.Args[0];
            Gst.State oldState, newState, pendingState;
            msg.ParseStateChanged(out oldState, out newState, out pendingState);
            if (msg.Src == LivePlaybin)
            {
                State = newState;
                Console.WriteLine("State set to {0}", Gst.Element.StateGetName(newState));

            }

        }
        // This function is called when an "application" message is posted on the bus. Here we retrieve the message posted by the HandleTags callback
        void HandleApplication(object sender, GLib.SignalArgs args)
        {
            var msg = (Gst.Message)args.Args[0];

        }
        Gst.State State;

        private Gst.Pipeline PlayBackPlaybin;
        private Gst.Element convert;
        public void StopVideoStreamRecording()
        {
            PlayBackPlaybin.SetState(Gst.State.Paused);

        }
        public bool RecordLiveVideoStream(string camName, string url, string dir)
        {
            try
            {

                if (string.IsNullOrEmpty(dir))
                {
                    dir = "C:/video-" + System.DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
                var MainLoop = new GLib.MainLoop();
                var m_GlibThread = new System.Threading.Thread(MainLoop.Run);

                // Create the elements
                var source = ElementFactory.Make("uridecodebin", "uridecodebin");
                convert = ElementFactory.Make("videoconvert", "videoconvert");
                var enc = ElementFactory.Make("x264enc", "enc");
                var mux = ElementFactory.Make("mpegtsmux", "mux");
                var sink = ElementFactory.Make("filesink", "sink");

                // Create the elements              
                PlayBackPlaybin = new Pipeline("test-pipeline");

                if (source == null || convert == null || sink == null || enc == null || mux == null || PlayBackPlaybin == null)
                {
                    Console.WriteLine("Not all elements could be created");
                    return false;
                }
                PlayBackPlaybin.Add(source, convert, enc, mux, sink);
                if (!convert.Link(enc) || !enc.Link(mux) || !mux.Link(sink))
                {
                    Console.WriteLine("Elements could not be linked");
                    return false;
                }
                // Set the URI to play
                source["uri"] = url;
                source.PadAdded += HandlePadAdded;
                sink["location"] = $"{dir}/{camName}-{System.DateTime.Now.ToString("HHmmss")}.mp4";

                var bus = PlayBackPlaybin.Bus;
                bus.EnableSyncMessageEmission();
                ////bus.SyncMessage += Bus_SyncMessage;
                bus.AddSignalWatch();

                // Start playing
                var ret = PlayBackPlaybin.SetState(Gst.State.Playing);

                m_GlibThread.Start();

                if (ret == Gst.StateChangeReturn.Failure)
                {
                    PlayBackPlaybin.SetState(Gst.State.Null);
                    return false;
                }

            }
            catch (Exception ex)
            {

            }
            return true;
        }
        public void HandlePadAdded(object o, PadAddedArgs args)
        {
            var src = (Element)o;
            var newPad = args.NewPad;
            var sinkPad = convert.GetStaticPad("sink");

            Console.WriteLine(string.Format("Received new pad '{0}' from '{1}':", newPad.Name, src.Name));

            // If our converter is already linked, we have nothing to do here
            if (sinkPad.IsLinked)
            {
                Console.WriteLine("We are already linked. Ignoring.");
                return;
            }

            // Check the new pad's type
            var newPadCaps = newPad.Caps;
            var newPadStruct = newPadCaps.GetStructure(0);
            var newPadType = newPadStruct.Name;


            // Attempt the link
            var ret = newPad.Link(sinkPad);
            if (ret != PadLinkReturn.Ok)
                Console.WriteLine("Type is '{0} but link failed.", newPadType);
            else
                Console.WriteLine("Link succeeded (type '{0}').", newPadType);
        }
    }
}
