using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRIM.SDK;
using System.IO;

// This source code provides an example of how to write a TRIM event processor
// Addin.  

// To configure this in TRIM, you would open TRIM Enterprise Studio,
// select the dataset you want the add-in to run for and choose the 
// Event Processsing -> Configure option on the right mouse menu.

// Click on the Custom Processes Tab and then press Add.
// Give your processor a name (can be anything, just there for readability)
// and then select the "Call a .Net Assembly" option and fill in the values
// for Assembly Name (here, I use TRIM.SDK.Samples.dll) and
// for Class Name (here, I use EventProcessor).

// Down the bottom, make sure processing is Enabled and select a workgroup to run it 
// on (if you select any workgroup, you will need to deploy your add-in to all
// running TRIM workgroups.

// For testing/debugging, you should start up the workgroup, which will launch a 
// copy of TRIMEvent for each TRIM data set configured (life is easier if you only have
// one dataset).  You should then attach your debugger to the TRIMevent.exe process, which
// will be the process that calls into your assembly.

// Having all that set up, fire up a copy of TRIM - just doing that will create a user logged on
// event, which should eventually hit your shores.


namespace TestEventProcessor
{
    public class EventProcessor : TrimEventProcessorAddIn
    {
        public override void ProcessEvent(Database db, TrimEvent eventData)
        {
            if (eventData.ObjectType == BaseObjectTypes.Record)
            {
                Record record = db.FindTrimObjectByUri(BaseObjectTypes.Record, eventData.ObjectUri) as Record;

                if (eventData.EventType == Events.ObjectAdded)
                {
                    record.Title = "THIS TITLE WAS SET BY THE TEST EVENT PROCESSOR";
                }
                else if (eventData.EventType == Events.ObjectModified)
                {
                    record.SetNotes("OBJECT MODIFIED", NotesUpdateType.AppendWithNewLine);
                }

                try
                {
                    record.Save();
                }
                catch (Exception e)
                {
                }

                if (eventData.ExtraDetails.Length > 0)
                {
                }
            }
        }
    }
}