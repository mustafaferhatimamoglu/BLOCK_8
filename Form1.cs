namespace BLOCK_8
{
    public partial class Form1 : Form
    {
        private void Setups()
        {

        }
        public Form1()
        {
            InitializeComponent();
            Setups();
            this.Load += This_Load;
        }

        private void This_Load(object? sender, EventArgs e)
        {
            CommonFunctions.Form.Setup_Form_Right_2K(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1280-1400
            //1294-1407
        }
    }
}
