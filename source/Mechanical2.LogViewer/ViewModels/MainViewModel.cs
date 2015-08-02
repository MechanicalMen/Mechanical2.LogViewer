using System;
using System.Collections.Generic;
using Mechanical.Logs;
using Mechanical.MVVM;

namespace Mechanical.LogViewer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public class LogEntryVM
        {
            public LogEntryVM( int index, LogEntry entry )
            {
                this.Index = index;
                this.LogEntry = entry;
            }

            public int Index
            {
                get;
                private set;
            }

            public LogEntry LogEntry
            {
                get;
                private set;
            }

            public DateTime LocalTimestamp
            {
                get { return this.LogEntry.Timestamp.ToLocalTime(); }
            }

            public bool LevelIsDebug
            {
                get { return this.LogEntry.Level == LogLevel.Debug; }
            }

            public bool LevelIsInformation
            {
                get { return this.LogEntry.Level == LogLevel.Information; }
            }

            public bool LevelIsWarning
            {
                get { return this.LogEntry.Level == LogLevel.Warning; }
            }

            public bool LevelIsError
            {
                get { return this.LogEntry.Level == LogLevel.Error; }
            }

            public bool LevelIsFatal
            {
                get { return this.LogEntry.Level == LogLevel.Fatal; }
            }
        }

        private List<LogEntryVM> entries;
        public List<LogEntryVM> Entries
        {
            get
            {
                return this.entries;
            }
            private set
            {
                if( !object.ReferenceEquals(this.entries, value) )
                {
                    this.entries = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public void LoadXmlFile( string filePath )
        {
            try
            {
                var fixedFileContents = XmlLogPatchUp.CloseOpenTags(filePath);
                using( var reader = new Mechanical.DataStores.Xml.XmlDataStoreReader(new System.IO.StringReader(fixedFileContents)) )
                {
                    var loaded = LogEntrySerializer.Deserialize(reader);

                    var newEntries = new List<LogEntryVM>(capacity: loaded.Count);
                    for( int i = 0; i < loaded.Count; ++i )
                        newEntries.Add(new LogEntryVM(i, loaded[i]));

                    this.Entries = newEntries;
                }
            }
            catch( Exception ex )
            {
                var str = Mechanical.Core.SafeString.DebugPrint(ex);
                str.ToString();
                throw;
            }
        }
    }
}
