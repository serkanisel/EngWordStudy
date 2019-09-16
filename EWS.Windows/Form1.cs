using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWS.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak(textBox1.Text);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text ="Dana veli nasılsın inşallah kardeş 35 madam! x'Türkiye";

            var matches = Regex.Matches(text, @"([A-z])\w+");


            foreach (var item in matches)
            {
                MessageBox.Show(item.ToString());
            }
            string result = String.Join(" ", matches.Cast<Match>().Select(m => m.Value));

            textBox2.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sentence = "Dana veli nasılsın inşallah kardeş 35 madam! x'Türkiye";
            //
            // Get all words.
            //
            string[] uppercaseWords = Regex.Split(sentence, @"");

            var matches = Regex.Matches(sentence, @"\W");

            // Get all uppercased words.
            //
            var list = new List<string>();
            foreach (string value in uppercaseWords)
            {
                //
                // Check the word.
                //
                if (!string.IsNullOrEmpty(value) &&
                    char.IsUpper(value[0]))
                {
                    list.Add(value);
                }
            }
            //
            // Write all proper nouns.
            //
            foreach (var value in list)
            {
                Console.WriteLine(value);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
