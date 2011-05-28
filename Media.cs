using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LittleGeek
{
    public abstract class Media : IContent
    {
        private int id;
        private int contentID;
        private string fileName;
        private string title;
        private string caption;
        private string description;
        private string metadata;
        private string recipientAddress;
        private string senderAddress;
        private Types.MediaTypes mediaType;

        private DateTime dateSubmitted;

        public Media()
        {

        }

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int ContentID
        {
            get { return this.contentID; }
            set { this.contentID = value; }
        }

        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public DateTime DateSubmitted
        {
            get { return this.dateSubmitted; }
            set { this.dateSubmitted = value; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Caption
        {
            get { return this.caption; }
            set { this.caption = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string Metadata
        {
            get { return this.metadata; }
            set { this.metadata = value; }
        }

        public string SenderAddress
        {
            get { return this.senderAddress; }
            set { this.senderAddress = value; }
        }

        public string RecipientAddress
        {
            get { return this.recipientAddress; }
            set { this.recipientAddress = value; }
        }

        public Types.MediaTypes MediaType
        {
            get { return this.mediaType; }
            set { this.mediaType = value; }
        }

        /// <summary>
        /// Saves descriptive information in the backing store (e.g, the database).
        /// </summary>
        public void Create()
        {
            DataConnector dc = new DataConnector();
            DataSet ds = dc.GetSet(SQLString.CreateContent(Types.ContentTypes.Media.GetHashCode(), 1));

            this.contentID = Convert.ToInt32(ds.Tables[0].Rows[0]["ContentID"]);

            ds = dc.GetSet(SQLString.CreateMedia(this));

            // If a title's been set, use it
            if (this.Title != String.Empty)
            {
                dc.ExecuteSql(SQLString.CreateTitle(this));
            }

            // If a caption's been set, use that
            if (this.Caption != String.Empty)
            {
                dc.ExecuteSql(SQLString.CreateCaption(this));
            }
        }

        public void SetTitle(string title)
        {

        }

        public void SetCaption(string Caption)
        {

        }

        public void SetDescription(string description)
        {

        }

        public void SetMetaData(string metaData)
        {

        }

        public void SetMediaInfo(Types.MediaInfoTypes infoType, string info)
        {
            DataConnector dc = new DataConnector();
            dc.ExecuteSql(SQLString.CreateMediaInfo(this, infoType, info));
        }
    }
}
