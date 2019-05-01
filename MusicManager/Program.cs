using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MusicManager
{
    class Program
    {
        private enum MainMenuUserChoice { Invalid = -1, None, ChangeArtist, EraseMetadata, ModifySongsFileName, GenerateInfoFile, UpdateMetaData, ExitProgram };
        private enum ModifySongsFileNameUserChoice { Invalid = -1, None, PrintFileNames, RemoveXFirstCharactere, Add, Remove, ContinueWithoutSaving, ContinueAndSave };

        static private Artist artist = new Artist();
        static private List<string> stringsToRemoveFromFileName = new List<string>();

        static private List<string> supportedExtensionLst = new List<string>() { ".mp3" };
        static int charactereToRemoveFromStart = 0;


        static void Main(string[] args)
        {
            ChangeArtist(@"C:\Users\mathi\Desktop\Musique\Burzum");

            MainMenu();
        }

        //Done
        static void MainMenu()
        {
            MainMenuUserChoice mainMenuUserChoice;

        MainMenu:
            Console.Clear();
            Console.Write("Current Artist Folder: " + artist.Path + "\n");
            Console.Write("1-Change Artist\n2-Erase Metadata\n3-Modify Songs File Name\n4-Generate Albums Info Files\n5-Update MetaData\n6-Exit Program\n");

            mainMenuUserChoice = ReadMainMenuUserChoice();

            switch (mainMenuUserChoice)
            {
                case MainMenuUserChoice.ChangeArtist:
                    ChangeArtist("");
                    break;

                case MainMenuUserChoice.EraseMetadata:
                    EraseMetadata();
                    break;

                case MainMenuUserChoice.ModifySongsFileName:
                    ModifySongsFileName();
                    break;

                case MainMenuUserChoice.GenerateInfoFile:
                    GenerateInfoFile();
                    break;

                case MainMenuUserChoice.UpdateMetaData:
                    UpdateMetaData();
                    break;

                case MainMenuUserChoice.ExitProgram:
                    Environment.Exit(1);
                    break;

                case MainMenuUserChoice.Invalid:
                case MainMenuUserChoice.None:
                default:
                    break;
            }

            goto MainMenu;
        }

        #region MainMenu

        //Done
        static void ChangeArtist(string artistDirectory)
        {
            if (artistDirectory == "")
            {
                Console.Clear();
                Console.Write("Enter the artist directory\nEx: Z:\\Musique\\Artist\n");
                artist.Path = Console.ReadLine();
            }
            else
            {
                artist.Path = artistDirectory;
            }

            try
            {
                foreach (string albumPath in Directory.GetDirectories(artist.Path))
                {
                    Album album = new Album(albumPath);

                    //TODO: Check for album cover

                    artist.AlbumLst.Add(album);

                    foreach (string songPath in Directory.GetFiles(album.Path, "*.mp3", SearchOption.AllDirectories))
                    {
                        Song song = new Song(songPath);
                        song.ModifiedName = song.Name;

                        album.SongLst.Add(song);
                    }
                }
            }
            catch { }
            
        }

        //Done
        static void EraseMetadata()
        {
            /*Mp3Lib.Mp3File mp3File;

            Console.Clear();
            Console.Write(artist.Name + "\n\n");

            foreach (Album album in artist.AlbumLst)
            {
                Console.Write("   " + album.Name + "\n");

                foreach (Song song in album.SongLst)
                {
                    Console.Write("      " + song.Name + "\n");

                    mp3File = new Mp3Lib.Mp3File(song.Path);
                    mp3File.
                    mp3File.TagHandler.Artist = "";
                    mp3File.TagHandler.Composer = "";
                    mp3File.TagHandler.Album = "";
                    mp3File.TagHandler.Title = "";
                    mp3File.TagHandler.Year = "";
                    mp3File.TagHandler.Track = "";
                    mp3File.Update();
                }
                Console.Write("\n");
            }
            Console.Write("Done!\nPress any key to terminate");*/
        }
        
        //Done
        #region ModifySongsFileName
        //Done
        static void ModifySongsFileName()
        {
        ModifySongsFileName:
            Console.Clear();

            Console.Write("\n1-Print File Names\n2-Remove X First Characteres\n3-Add strings\n4-Delete strings\n5-Continue\n6-Rename files and continue\n");

            switch (ReadModifySongsFileNameUserChoice())
            {
                case ModifySongsFileNameUserChoice.PrintFileNames:
                    UpdateSongModifiedName();
                    PrintFileNames();
                    Console.ReadLine();
                    goto ModifySongsFileName;

                case ModifySongsFileNameUserChoice.RemoveXFirstCharactere:
                    RemoveXFirstCharactere();
                    UpdateSongModifiedName();
                    goto ModifySongsFileName;

                case ModifySongsFileNameUserChoice.Add:
                    AddStringMenu();
                    UpdateSongModifiedName();
                    goto ModifySongsFileName;

                case ModifySongsFileNameUserChoice.Remove:
                    RemoveStringMenu();
                    UpdateSongModifiedName();
                    goto ModifySongsFileName;

                case ModifySongsFileNameUserChoice.ContinueWithoutSaving:
                    break;

                case ModifySongsFileNameUserChoice.ContinueAndSave:
                    SaveFileModification();
                    break;

                default:
                    goto ModifySongsFileName;
            }
        }

        //Done
        static void PrintFileNames()
        {
            Console.Clear();
            Console.Write(artist.Name + "\n");

            foreach (Album album in artist.AlbumLst)
            {
                Console.Write("   " + album.Name + "\n");

                foreach (Song song in album.SongLst)
                    Console.Write("      " + song.ModifiedName + "\n");

                Console.Write("\n");
            }
        }

        //Done
        static void RemoveXFirstCharactere()
        {
            Console.Clear();
            Console.Write("How Many charactere:\n");

            if (!Int32.TryParse(Console.ReadLine(), out charactereToRemoveFromStart))
            {

            }
        }

        //Done
        static void AddStringMenu()
        {
            string consoleInputTemp;

            Console.Clear();

            PrintFileNames();

            Console.Write("Enter a string to add or press \"Enter\" to exit\n");

            for (int i = 0; i < stringsToRemoveFromFileName.Count(); i++)
                Console.Write(stringsToRemoveFromFileName[i] + "\n");

            do
            {
                consoleInputTemp = Console.ReadLine();

                if (consoleInputTemp != "")
                    stringsToRemoveFromFileName.Add(consoleInputTemp);

            } while (consoleInputTemp != "");
        }

        //Done
        static void RemoveStringMenu()
        {

            string consoleInputTemp;
            int toDelete;

            Console.Clear();

            do
            {
                Console.Write("Choose a string to delete or press \"Enter\" to exit\n");

                for (int i = 0; i < stringsToRemoveFromFileName.Count(); i++)
                    Console.Write(i.ToString() + "-" + stringsToRemoveFromFileName[i] + "\n");

                consoleInputTemp = Console.ReadLine();

                if (int.TryParse(consoleInputTemp, out toDelete))
                    stringsToRemoveFromFileName.RemoveAt(toDelete);

            } while (consoleInputTemp != "");
        }
        
        //Done
        static void SaveFileModification()
        {
            Console.Write("\nAre you sure you really want to rename those files?\nchanges will be !!!PERMANENT!!!\nType \"Enter\" or press any key for \"No\"\n");
            if (Console.ReadLine() == "")
            {
                Console.Write(artist.Name + "\n");

                foreach( Album album in artist.AlbumLst)
                {
                    string fileDirectory;

                    Console.Write("   " + album.Name + "\n");

                    foreach(Song song in album.SongLst)
                    {
                        fileDirectory = song.Path.Replace(song.FileNameExtension, "");

                        File.Move(fileDirectory + song.FileNameExtension, fileDirectory + song.ModifiedNameExtension);

                        song.Name = song.ModifiedName;

                        Console.Write("      " + song.Name + "\n");
                    }
                    Console.Write("\n");
                }
                Console.Write("Done!");
            }

            Console.ReadLine();
        }

        #endregion //ModifySongsFileName

        //Done
        static void GenerateInfoFile()
        {
            UpdateSongModifiedName();

            Console.Clear();
            Console.Write("Creating \"Album\"_Info.txt files\n\n");
            Console.Write(artist.Name + "\n");

            foreach (Album album in artist.AlbumLst)
            {
                string infoFileNameAndExtension = "";
                string writeToFile = "";

                infoFileNameAndExtension = Path.GetFileName(album.Path); ;
                infoFileNameAndExtension += "_Info.txt";

                if (!File.Exists(album.Path + "\\" + infoFileNameAndExtension))
                {
                    File.Create(album.Path + "\\" + infoFileNameAndExtension).Close();

                    writeToFile = "Year:" + Environment.NewLine;

                    int i = 0;
                    foreach (Song song in album.SongLst)
                    {
                        writeToFile += "|" + song.FileNameExtension;
                        if(i != album.SongLst.Count -1)
                            writeToFile += Environment.NewLine;
                        i++;
                    }
                    
                    //File.WriteAllText("   " + album.Path + "\\" + infoFileNameAndExtension, writeToFile);
                    File.WriteAllText(album.Path + "\\" + infoFileNameAndExtension, writeToFile);

                    Console.Write("   " + infoFileNameAndExtension + " created!\n");
                }
                else Console.Write("   " + infoFileNameAndExtension + " already exist!\n");
            }

            Console.Write("\nDone!\n");
            Console.ReadLine();
        }

        #region UpdateMetaData

        //Done
        static void UpdateMetaData()
        {
            Console.Write("\nUpdate songs metadata?\nPress \"Enter\" for \"Yes\" or Type \"No\"\n");

            if (Console.ReadLine() == "")
            {
                ReadInfoFileAndCover();

                Mp3Lib.Mp3File mp3File;

                Console.Clear();
                Console.Write(artist.Name + "\n\n");

                foreach (Album album in artist.AlbumLst)
                {
                    Console.Write("   " + album.Name + "\n");

                    foreach (Song song in album.SongLst)
                    {
                        Console.Write("      " + song.Name + "\n");

                        mp3File = new Mp3Lib.Mp3File(song.Path);
                        mp3File.TagHandler.Artist = artist.Name;
                        mp3File.TagHandler.Composer = artist.Name;
                        mp3File.TagHandler.Album = album.Name;
                        mp3File.TagHandler.Title = song.Name;
                        mp3File.TagHandler.Year = album.Year;
                        mp3File.TagHandler.Track = song.Track;
                        if (album.Name != "Autre")
                            mp3File.TagHandler.Picture = album.Cover;

                        /*Pour les erreur suivantes:
                         *  System.IO.EndOfStreamException : 'Impossible de lire au-delà de la fin du flux.'
                         *      Effacer le metadata du fichier en question
                         *      
                         */
                        mp3File.Update();
                    }
                    Console.Write("\n");
                }
                Console.Write("Done!\nPress any key to terminate");
            }
            Console.ReadLine();
        }

        static void ReadInfoFileAndCover()
        {
            //TODO: Implementer un mechanisme de formatage pour lire et ecrire le fichier Info_"Album".txt
            foreach (Album album in artist.AlbumLst)
            {
                List<string> contents = File.ReadAllText(album.Path + @"\" + album.Name + "_Info.txt").Replace("\r\n", "_").Split('_').ToList();
                album.Year = contents.ElementAt(0).Split(':').Last();

                //TODO: Gerer l'erreur si il ny a pas d'image et avertir l'usager
                album.Cover = Image.FromFile(album.Path + "\\" + album.Name + ".jpg");


                for(int i=1; i< contents.Count; i++)
                {
                    Song song  = album.SongLst.Find(x => x.FileNameExtension == contents.ElementAt(i).Replace(contents.ElementAt(i).Split('|').First() + "|", ""));
                    if (song != null)
                    {
                        song.Track = contents.ElementAt(i).Split('|').First();
                    }
                    else
                    {
                        Console.Write("Song title (" + contents.ElementAt(i).Split('|').Last() + ") in " + album.Name + "_Info.txt can't be found in " + album.Name + " directory\n");
                        Console.ReadLine();
                    }
                }
            }
        }
        #endregion // UpdateMetaData

        #endregion //MainMenu

        //Done
        static ModifySongsFileNameUserChoice ReadModifySongsFileNameUserChoice()
        {
            ModifySongsFileNameUserChoice userChoice;

            if (!Int32.TryParse(Console.ReadLine(), out int choice))
                return ModifySongsFileNameUserChoice.Invalid;

            userChoice = (ModifySongsFileNameUserChoice)choice;

            switch (userChoice)
            {
                case ModifySongsFileNameUserChoice.PrintFileNames:
                case ModifySongsFileNameUserChoice.RemoveXFirstCharactere:
                case ModifySongsFileNameUserChoice.Add:
                case ModifySongsFileNameUserChoice.Remove:
                case ModifySongsFileNameUserChoice.ContinueAndSave:
                case ModifySongsFileNameUserChoice.ContinueWithoutSaving:
                    return userChoice;

                case ModifySongsFileNameUserChoice.None:
                case ModifySongsFileNameUserChoice.Invalid:
                default:
                    return ModifySongsFileNameUserChoice.Invalid;

            }
        }

        //Done
        static MainMenuUserChoice ReadMainMenuUserChoice()
        {
            MainMenuUserChoice userChoice;

            if (!Int32.TryParse(Console.ReadLine(), out int choice))
                return MainMenuUserChoice.Invalid;

            userChoice = (MainMenuUserChoice)choice;

            switch (userChoice)
            {
                case MainMenuUserChoice.ChangeArtist:
                case MainMenuUserChoice.EraseMetadata:
                case MainMenuUserChoice.ModifySongsFileName:
                case MainMenuUserChoice.GenerateInfoFile:
                case MainMenuUserChoice.UpdateMetaData:
                case MainMenuUserChoice.ExitProgram:
                    return userChoice;

                case MainMenuUserChoice.None:
                case MainMenuUserChoice.Invalid:
                default:
                    return MainMenuUserChoice.Invalid;
            }
        }

        //Done
        static void UpdateSongModifiedName()
        {
            foreach (Album album in artist.AlbumLst)
                foreach (Song song in album.SongLst)
                {
                    string songNameTemp = "";

                    songNameTemp = song.ModifiedName;

                    songNameTemp = songNameTemp.Replace(album.Name, "");

                    if (songNameTemp.Length >= charactereToRemoveFromStart)
                        for (int i=charactereToRemoveFromStart; i>0; i--)
                            songNameTemp = songNameTemp.Remove(0, 1);

                    foreach (string toRemove in stringsToRemoveFromFileName)
                        songNameTemp = songNameTemp.Replace(toRemove, "");

                    if (songNameTemp.Length > 0)
                        while (songNameTemp[0] == ' ')
                            songNameTemp = songNameTemp.Remove(0, 1);

                    if (songNameTemp.Length > 0)
                        while (songNameTemp[songNameTemp.Length - 1] == ' ')
                            songNameTemp = songNameTemp.Remove(songNameTemp.Length - 1, 1);

                    if (songNameTemp == "")
                        songNameTemp = album.Name;

                    song.ModifiedName = songNameTemp;
                }

            charactereToRemoveFromStart = 0;
        }
    }
}
