using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MusicManager
{
    class Artist
    {
        private string path = "";
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                name = System.IO.Path.GetFileName(path);
            }
        }

        private string name = "";
        public string Name { get { return name; } }

        public List<Album> AlbumLst = new List<Album>();

        public Artist()
        {

        }

        public Artist(string path)
        {
            Path = path;
        }

    }

    class Album
    {
        private Image cover;
        public Image Cover{ get { return cover; } set { cover = value; } }

        private string path = "";
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                name = System.IO.Path.GetFileName(path);
            }
        }

        private string name = "";
        public string Name { get { return name; } }

        private string year = "";
        public string Year { get { return year; } set { year = value; } }

        public List<Song> SongLst = new List<Song>();

        public Album() { }

        public Album(string path)
        {
            Path = path;
        }
    }

    class Song
    {
        private string path = "";
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                name = System.IO.Path.GetFileNameWithoutExtension(path);
                fileNameExtension = System.IO.Path.GetFileName(path);
                fileExtension = System.IO.Path.GetExtension(path);
            }
        }

        private string name = "";
        public string Name {
            get { return name; }
            set {
                name = value;
                path = System.IO.Path.GetDirectoryName(path) + value;
                fileNameExtension = value + fileExtension;
            }
        }

        private string fileNameExtension = "";
        public string FileNameExtension { get { return fileNameExtension; } }

        private string modifiedName = "";
        public string ModifiedName {
            get { return modifiedName; }
            set {
                modifiedName = value;
                modifiedNameExtension = value + fileExtension;
            }
        }

        private string modifiedNameExtension = "";
        public string ModifiedNameExtension { get { return modifiedNameExtension; } }

        private string fileExtension = "";
        public string FileExtension { get { return fileExtension; } }

        private string track = "";
        public string Track
        {
            get { return track; }
            set { track = value; }
        }

        public Song(){ }

        public Song(string path)
        {
            Path = path;
        }
    }
}
