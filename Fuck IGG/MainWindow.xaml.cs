using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Net;
using System.Windows.Shapes;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fuck_IGG {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        public JObject gamesObject = new JObject();
        private string PatchesUrl = "https://raw.githubusercontent.com/pheeeeenom/fuckigg/master/database/patches.json";
        public MainWindow() {
            InitializeComponent();
            string patches;

            if (File.Exists("patches.json")) {
                // Read File
                patches = File.ReadAllText("patches.json");
            } else {
                try {
                    using (WebClient wc = new WebClient()) {
                        // Download the patches.json from GitHub
                        string onlinePatchesString = wc.DownloadString(PatchesUrl);
                        JObject webPatches = JsonConvert.DeserializeObject<JObject>(onlinePatchesString);

                        JArray defaultPatches = new JArray();
                        var gamesObj = (JObject)webPatches;
                        foreach (var gameObj in gamesObj) {
                            JObject defaultPatch = new JObject(
                            new JProperty("Name", gameObj.Key),
                            new JProperty("Exec_name", gamesObj[gameObj.Key]["Exec_name"]),
                            new JProperty("Offset", gamesObj[gameObj.Key]["Offset"]),
                            new JProperty("Patches", gamesObj[gameObj.Key]["Exec_namePatches"]));
                            defaultPatches.Add(defaultPatch);
                        }
                        var file = File.CreateText("patches.json");
                        patches = defaultPatches.ToString();
                        Console.WriteLine(patches);

                        var json = JsonConvert.SerializeObject(defaultPatches, Formatting.Indented);
                        file.Write(json);
                        file.Close();
                    }
                } catch (WebException e) {
                    Console.WriteLine("Error download online patches... Using default");
                    Console.WriteLine(e);
                    // Create File
                    Console.WriteLine("No patches.json found, creating new Patches.json");
                    JArray defaultPatches = new JArray();

                    JObject defaultPatch = new JObject(
                        new JProperty("Name", "Moekuri: Adorable + Tactical SRPG"),
                        new JProperty("Exec_name", "moekurii.exe"),
                        new JProperty("Offset", "0x2B9"),
                        new JProperty("Patches", "0x3A"));
                    defaultPatches.Add(defaultPatch);
                    patches = defaultPatches.ToString();

                    var file = File.CreateText("patches.json");

                    var json = JsonConvert.SerializeObject(defaultPatches, Formatting.Indented);
                    file.Write(json);
                    file.Close();
                }
            }

            dynamic games = JsonConvert.DeserializeObject(patches);
            foreach (var game in games) {
                GameListElement.Children.Add(new System.Windows.Controls.RadioButton {
                    Content = game.Name,
                    GroupName = "GameButtons",
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 14.0,
                    Margin = new Thickness(15, 5, 15, 0),
                    Foreground = Brushes.White
                });
                Console.WriteLine("{0} {1} {2} {3}", game.Name, game.Exec_name, game.Offset, game.Patches);
                JObject gameObject = new JObject();
                gameObject["Name"] = (string)game.Name;
                gameObject["Exec_name"] = (string)game.Exec_name;
                gameObject["Offset"] = (string)game.Offset;
                gameObject["Patches"] = (string)game.Patches;
                gamesObject[game.Name.ToString()] = gameObject;
            }
        }

        private void Patch_Click(object sender, RoutedEventArgs e) {
            Warning_Label.Opacity = 0;
            Exe_Label.Opacity = 0;
            Db_Exe_Label.Opacity = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.exe";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Console.WriteLine(openFileDialog.FileName);
                string dir = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                string[] garbage = Directory.GetFiles(dir);

                foreach (string file in garbage)
                {
                    if (file.Contains("IGG") || file.Contains("GAMESTORRENT.CO"))
                    {
                        File.Delete(file);
                    }
                }

                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);

                string game_exec = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1, openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1);
                var checkedButton = GameListElement.Children.OfType<System.Windows.Controls.RadioButton>().FirstOrDefault(r => r.IsChecked ?? false);
                Console.WriteLine(gamesObject[checkedButton.Content.ToString()]["Exec_name"]);
                Console.WriteLine(game_exec);
                if (game_exec == (string)gamesObject[checkedButton.Content.ToString()]["Exec_name"]) {
                    Console.Write("Game: {0} matches exec: {1}\n", checkedButton.Content, game_exec);
                    int offset = Convert.ToInt32(gamesObject[checkedButton.Content.ToString()]["Offset"].ToString(), 16);
                    int patches = Convert.ToInt32(gamesObject[checkedButton.Content.ToString()]["Patches"].ToString(), 16);
                    bw.BaseStream.Position = offset;
                    bw.Write((byte)patches);
                } else {
                    Console.Write("No match for Game: {0}\n", checkedButton.Content);
                    Warning_Label.Opacity = 100;
                    Exe_Label.Opacity = 100;
                    Exe_Label.Content = "Selected: " + checkedButton.Content;
                    Db_Exe_Label.Opacity = 100;
                    Db_Exe_Label.Content = "Database: " + (string)gamesObject[checkedButton.Content.ToString()]["Exec_name"];
                }

                bw.Flush();
                fs.Flush();
                bw.Close();
                fs.Close();
            }
        }

        private void GameListElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(GameListElement.Children.OfType<System.Windows.Controls.RadioButton>().Any(rb => rb.IsChecked == true)))
            {
                Patch_Btn.IsEnabled = true;
            }
        }
    }
}
