using System.Text;
using System.Windows.Forms;

namespace Codio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Init memory var for undo
        public string? mem1 = null; //Init mem 1 variable for undo action
        public bool isSingle = false; // Init isSingle bool for single list delete undo

        private int _list_counter = 0;

        public event System.EventHandler? VarChanged;

        //On list counter variable change update label
        protected virtual void OnVarChanged()
        {
            if (VarChanged != null) VarChanged(this, EventArgs.Empty);
            label2.Text = ListCounter.ToString();
        }

        //Defining List counter get, set
        public int ListCounter
        {
            get
            {
                return _list_counter;
            }
            set
            {
                _list_counter = value;
                OnVarChanged();

            }
        }

        //Creating array for undo action of listBox1
        public string[]? list_content;

        //Todo at form load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.LightBlue;  // Changing inner bg color of the window
            listBox1.MouseDown += ListBox1_MouseDown;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Submit button
        private void button1_Click(object sender, EventArgs e)
        {
            string text_parsed; // Define parsified text variable
            text_parsed = textBox1.Text;  // Storing var from texBox1
            if (text_parsed != "")
            {
                listBox1.Items.Add(text_parsed); // Add parsed text into listBox1
                ListCounter++; // Inc counter
            }
            textBox1.Clear();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        //Delete all button
        private void button2_Click(object sender, EventArgs e)
        {
            isSingle = false;
            mem1 = textBox1.Text;
            list_content = listBox1.Items.OfType<string>().ToArray(); // Storing all data form listBox1 for undo action
            textBox1.Clear();
            listBox1.Items.Clear();
            ListCounter = 0;
        }

        //Delete upper text
        private void button3_Click(object sender, EventArgs e)
        {
            mem1 = textBox1.Text; // Storing into mem1 what typed in textBox1
            textBox1.Clear();
        }


        //Undo last action
        private void button4_Click(object sender, EventArgs e)
        {
            if ((mem1 != null) && (list_content != null) && (isSingle == false))
            {
                textBox1.AppendText(mem1);
                mem1 = null;
                for (int i = 0; i < list_content.Length; i++) // Instert stored mem array back to lisBox1
                {
                    listBox1.Items.Add(list_content[i].ToString());
                    ListCounter++;
                }
                list_content = null;
            }
            else if ((mem1 != null) && (list_content != null) && (isSingle = true))
            {
                textBox1.AppendText(mem1);
                mem1 = null;
                listBox1.Items.Add(list_content[ListCounter].ToString()); // Instert stored mem last array item back to lisBox1
                ListCounter++;
                list_content = null;
            }
            else if ((list_content != null) && (isSingle == false))
            {
                for (int i = 0; i < list_content.Length; i++) // Instert stored mem array back to lisBox1
                {
                    listBox1.Items.Add(list_content[i].ToString());
                    ListCounter++;
                }
                list_content = null;
            }
            else if ((list_content != null) && (isSingle = true))
            {
                listBox1.Items.Add(list_content[ListCounter].ToString()); // Instert stored mem last array item back to lisBox1
                ListCounter++;
                list_content = null;
            }
            else if (mem1 != null)
            {
                textBox1.AppendText(mem1);
                mem1 = null;
            }
            else
            {
                Form2 f2 = new(); // Show error dialog
                f2.ShowDialog();
            }
        }

        //Delete last element added to lisBox1
        private void button5_Click(object sender, EventArgs e)
        {
            if (ListCounter > 0)
            {
                isSingle = true;
                list_content = listBox1.Items.OfType<string>().ToArray();
                listBox1.Items.RemoveAt(ListCounter - 1);
                ListCounter--;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ListBox1_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBox1.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBox1.SelectedIndex = index;
                    if (listBox1.SelectedItem != null)
                    {
                        object selectedItemText = listBox1.SelectedItem;
                        Clipboard.SetText((string)selectedItemText);
                    }
                }
                else
                {
                    Form2 f2 = new(); // Show error dialog
                    f2.ShowDialog();
                }
            }
        }

        static string GenerateKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int keyLength = 10;
            Random random = new Random();

            StringBuilder keyBuilder = new StringBuilder();
            for (int i = 0; i < keyLength; i++)
            {
                int index = random.Next(chars.Length);
                keyBuilder.Append(chars[index]);
            }

            return keyBuilder.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            const int numberOfKeys = 10;

            for (int i = 0; i < numberOfKeys; i++)
            {
                string key = GenerateKey();
                listBox1.Items.Add(key.ToString());
                ListCounter++;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Text file (*.txt)|*.txt";
                dlg.DefaultExt = "txt";
                dlg.AddExtension = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(dlg.FileName);

                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        writer.WriteLine((string)listBox1.Items[i]);
                    }



                    writer.Close();
                }

                dlg.Dispose();
            }
            else
            {
                Form3 f3 = new(); // Show error dialog
                f3.ShowDialog();
            }
        }
    }
}