using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        void myShowToolTip(TextBox tB, byte[] arr)
        {
            string binValues = BitConverter.ToString(arr);
            string result = string.Empty;
            //Код, що виводить підказку у двійковому коді
            foreach (char ch in arr)
            {
                result += Convert.ToString((int)ch, 2);
            }
            toolTip_BIN.SetToolTip(tB, result);
        }
        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        byte[] myXOR(byte[] arr_text, byte[] arr_key)
        {
            int len_text = arr_text.Length;
            int len_key = arr_key.Length;
            byte[] arr_cipher = new byte[len_text];
            for (int i = 0; i < len_text; i++)
            {
                byte p = arr_text[i];
                byte k = arr_key[i % len_key]; // mod
                byte c = (byte)(p ^ k); // XOR

                arr_cipher[i] = c;
            }
            return arr_cipher;
        }

        string myCipher(TextBox tb_text, TextBox tb_Key, TextBox tb_cipher, string cipher = "")
        {
            string text = tb_text.Text;
            byte[] arr_text;
            //GetEncoding(1251) - Код кирилиці
            if (cipher == "") arr_text = Encoding.GetEncoding(1251).GetBytes(text);
            else arr_text = Encoding.GetEncoding(1251).GetBytes(cipher);
            myShowToolTip(tb_text, arr_text); // Створити підказку

            string key = tb_Key.Text;
            byte[] arr_key = Encoding.GetEncoding(1251).GetBytes(key);
            myShowToolTip(tb_Key, arr_key); // Створити підказку

            byte[] arr_cipher = myXOR(arr_text, arr_key);

            cipher = Encoding.GetEncoding(1251).GetString(arr_cipher);
            tb_cipher.Text = cipher;
            myShowToolTip(tb_cipher, arr_cipher); // Створити підказку


            return cipher;
        }

        //
        // ifkeynull Потрібен для того щоб виводити підказку якщо ключа немає
        string ifkeynull(TextBox tb_text, TextBox tb_Key, TextBox tb_cipher, string cipher = "")
        {
            string text = tb_text.Text;
            byte[] arr_text;
            if (cipher == "") arr_text = Encoding.ASCII.GetBytes(text);
            else arr_text = Encoding.ASCII.GetBytes(cipher);
            myShowToolTip(tb_text, arr_text); // Створити підказку

            string key = tb_Key.Text;
            byte[] arr_key = Encoding.ASCII.GetBytes(key);
            myShowToolTip(tb_Key, arr_key); // Створити підказку

            return cipher;
        }

        private void button_XOR_Click(object sender, EventArgs e)
        {
            //Якщо ключа немає - textBox_C_OUT.Text = textBox_P_IN.Text
            //Також викликаємо ifkeynull
            if (string.IsNullOrEmpty(textBox_Key_IN.Text))
            {
                textBox_C_OUT.Text = textBox_P_IN.Text;

                string cipher = ifkeynull(textBox_P_IN, textBox_Key_IN, textBox_C_IN); 
                textBox_P_OUT.Text = textBox_C_IN.Text;
                textBox_Key_OUT.Text = textBox_Key_IN.Text;
                ifkeynull(textBox_P_OUT, textBox_Key_OUT, textBox_C_OUT, cipher);
            }
            // Якщо ключ є - шифруємо та дешифруємо
            else 
            {
                string cipher = myCipher(textBox_P_IN, textBox_Key_IN, textBox_C_IN); // зашифрування
                textBox_P_OUT.Text = textBox_C_IN.Text;
                textBox_Key_OUT.Text = textBox_Key_IN.Text;
                myCipher(textBox_P_OUT, textBox_Key_OUT, textBox_C_OUT, cipher); // розшифрування
            }
        }
        private void button_clean_Click(object sender, EventArgs e)
        {
            textBox_P_IN.Text = "";
            textBox_Key_IN.Text = "";
            textBox_C_IN.Text = "";

            textBox_P_OUT.Text = "";
            textBox_Key_OUT.Text = "";
            textBox_C_OUT.Text = "";

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
