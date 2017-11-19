using System;
using System.Data;
using System.Configuration;

namespace Common
{

/// <summary>
/// Summary description for Thumbnail
/// </summary>
    public class Thumbnail
    {
        public Thumbnail(string id, byte[] data)
        {
            this.ID = id;
            this.Data = data;
        }

        public Thumbnail(string id, string fileName, string format, byte[] data)
        {
            this.ID = id;
            this.FileName = fileName;
            this.Format = format;
            this.Data = data;
        }


        private string id;
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        private byte[] thumbnail_data;
        public byte[] Data
        {
            get
            {
                return this.thumbnail_data;
            }
            set
            {
                this.thumbnail_data = value;
            }
        }


        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string format;
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }
    }
}
