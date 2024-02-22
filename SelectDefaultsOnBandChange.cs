//INCLUDE_ASSEMBLY System.dll
//INCLUDE_ASSEMBLY System.Windows.Forms.dll

// Simple "Hello World" for K3. Completely based on the sample scripts in https://dxlog.net/sw/#files%2Fcustomization%2Fscripts
// 2024-02-21 John Ronan, EI7IG

using System;
using System.Windows.Forms;
using System.Threading;
//using IOComm;

namespace DXLog.net
{
    public class Script : ScriptClass
    {

        ContestData cdata;
        FrmMain frmMain;
        
        // Testing by calling main, simply returns the selected contest
        public void Initialize(FrmMain main)
        {
            cdata = main.ContestDataProvider;
            frmMain = main;
            cdata.ActiveRadioBandChanged += new ContestData.ActiveRadioBandChange(HandleActiveRadioBandChanged);
            main.SetMainStatusText(String.Format("Hello Dave!"));
            //Main(main, main.ContestDataProvider, main.COMMainProvider);
        } // End of Initialize

        public void Deinitialize()
        {
            data.ActiveRadioBandChanged -= HandleActiveRadioBandChanged;
        } // End of Deinitialize
        
        public void Main(FrmMain main, ContestData cdata, COMMain comMain)
        {
            String contestName = cdata.activeContest.cdata.ContestName;
            main.SetMainStatusText(String.Format("Info -> Mapped Function Key Pressed"));
            //main.SetMainStatusText(String.Format("Info -> Selected Contest name is {0}!", contestName));
        }
        public void HandleActiveRadioBandChanged(int radioNumber)
        {
            // get the radio object, if possible
            CATCommon radioObject = frmMain.COMMainProvider.RadioObject(radioNumber);

            // Initialise the command string
            string cmd = string.Empty;
            
            // Output error if there is no radio connected
            if (radioObject == null)
            {
                frmMain.SetMainStatusText(String.Format("ERROR: Unable to communicate with radio {0}!", radioNumber));
                return;
            } else{

                string radiomodel = (string)radioObject.GetType().GetField("RadioID").GetValue(null);
            	
                if (radiomodel.Contains("Elecraft K3/K3S"))
                {
                    // DV1 	- Diversity ON
		    radioObject.SendCustomCommand("DV1");
		    frmMain.SetMainStatusText(String.Format("Diversity On", radioNumber));
		    Thread.Sleep(1000);

		    // AR1 	- RX Antenna ON
		    radioObject.SendCustomCommand("AR1");
                    frmMain.SetMainStatusText(String.Format("Receive Antenna On", radioNumber));
                    Thread.Sleep(1000);

		    // RC  	- Rit Clear
		    radioObject.SendCustomCommand("RC");
                    frmMain.SetMainStatusText(String.Format("RIT Clear", radioNumber));
                    Thread.Sleep(1000);

		    // RG250	- Main RF Gain Maximum 
		    radioObject.SendCustomCommand("RG250");
                    frmMain.SetMainStatusText(String.Format("Main RF Gain Maximum", radioNumber));
                    Thread.Sleep(1000);

		    // RG$250	- Sub RF Gain Maximum
		    radioObject.SendCustomCommand("RG$250");
                    frmMain.SetMainStatusText(String.Format("Sub RF Gain Maximum", radioNumber));
                    Thread.Sleep(1000);

		    // SWH25	- Press and Hold RX Ant
		    radioObject.SendCustomCommand("SWH25");
                    frmMain.SetMainStatusText(String.Format("Sub Toggle Aux", radioNumber));
                    Thread.Sleep(1000);

                    //cmd = "DV1;AR1;RC;RG250;RG$250;SWH25";
                    //frmMain.SetMainStatusText(String.Format("Reset", radioNumber));
                }	
            }
            
            // send CAT commands (if any) and update status message
            //if (String.IsNullOrEmpty(cmd))
            //{
            //    frmMain.SetMainStatusText(String.Format("ERROR: Band change radio {0} ({1}), no action specified", radioNumber, radioObject.ToString()));
            //} else {
            //    radioObject.SendCustomCommand(cmd);
            //    frmMain.SetMainStatusText(String.Format("Band change radio {0} ({1}), Reset!", radioNumber, radioObject.ToString()));
            //}
        }
    }
}
