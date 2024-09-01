namespace BLOCK_8
{
    public partial class Form1 : Form
    {
        public void Setup_Form_Right()
        {
            Screen scr = Screen.FromPoint(this.Location);
            this.Location = new Point(scr.WorkingArea.Width / 2, scr.WorkingArea.Top);
            this.Width = scr.WorkingArea.Width / 2;
            this.Height = scr.WorkingArea.Height;
        }
        public void Setup_Form_Right_2K()
        {
            Screen scr = Screen.FromPoint(this.Location);
            this.Location = new Point(1275, 0);
            this.Width = 1292;
            this.Height = 1407;
        }
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
            Setup_Form_Right_2K();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1280-1400
            //1294-1407
        }
    }
}
