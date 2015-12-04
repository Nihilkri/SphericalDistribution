﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SphericalDistribution {
	public partial class Form1 : Form {
		#region Variables
		/// <summary>
		/// Random Number Generator
		/// </summary>
		public Random rng = new Random();
		/// <summary>
		/// fx,fy is the size of the form; fx2,fy2 is half that;
		/// so that (fx2, fy2) is the center of the form
		/// </summary>
		public int fx, fy, fx2, fy2;
		/// <summary>
		/// the canvas that holds the back buffer
		/// </summary>
		private Bitmap gi;
		/// <summary>
		/// The graphics for the back buffer that draws on gi
		/// </summary>
		private Graphics gb;
		/// <summary>
		/// The graphics for the front buffer that draws on the form
		/// </summary>
		private Graphics gf;
		/// <summary>
		/// The timer object for setting the heartbeat
		/// </summary>
		private Timer tim = new Timer() { Interval = 1000 / 30 };
		/// <summary>
		/// The start time for frame timings
		/// </summary>
		private DateTime st;
		/// <summary>
		/// The finish time for frame timings
		/// </summary>
		private TimeSpan ft;
		/// <summary>
		/// Total number of frames elapsed since start
		/// </summary>
		private static ulong _tfr = 0; public static ulong tfr { get { return _tfr; } }

		private static bool[] opt = new bool[10];
		private static int state = 1;

		struct Well { public float x, y; }
		private List<Well> wells = new List<Well>();

		const int    c  = 299792458; // Speed of light, in m/s
		const double G  = 6.673848e-11; // Gravitational Constant, in m3/kg/s2 or N(m/kg)2
		double Fg(double m1, double m2, double r) { return G * ((m1 * m2) / (r * r)); } // Force due to Gravity

		#endregion Variables
		#region Events
		public Form1() { InitializeComponent(); }
		private void Form1_Load(object sender, EventArgs e) {
			Width = 1600; Left = 0;
			Width = Screen.GetBounds(this).Width;
			Height = Screen.GetBounds(this).Height;
			fx = Width; fy = Height; fx2 = fx / 2; fy2 = fy / 2;
			gi = new Bitmap(fx, fy); gb = Graphics.FromImage(gi);
			gf = CreateGraphics(); tim.Tick += tim_Tick;

			for(int q = 0 ; q < 10 ; q++) opt[q] = false;

			tim.Start();
			//Calc();
		}
		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			switch(e.KeyCode) {
				case Keys.Escape: Close(); return;
				case Keys.Space: tim.Enabled = !tim.Enabled; Calc(); break;
				case Keys.Left:  _tfr--; Calc(); break;
				case Keys.Right: _tfr++; Calc(); break;
				case Keys.Oemtilde: for(int q = 0 ; q < 10 ; q++) opt[q] = false; break;
				case Keys.Back: if(wells.Count > 0) wells.RemoveAt(wells.Count - 1); break;

				default:
					int n = -1;
					if(e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) n = e.KeyCode - Keys.D1;
					if(e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9) n = e.KeyCode - Keys.NumPad1;
					if(e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0) n = 9;
					if(n > -1) { opt[n] = !opt[n]; if(!tim.Enabled) Calc(); }
					break;
			}
		}
		private void Form1_MouseClick(object sender, MouseEventArgs e) {
			if(wells.Count < 10) wells.Add(new Well() { x = e.X, y = fy2 - e.Y }); Calc();
		}
		private MouseEventArgs mouse = new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, 0, 0, 0);
		private void Form1_MouseMove(object sender, MouseEventArgs e) {
			mouse = e;
		}
		private void Form1_Paint(object sender, PaintEventArgs e) { gf.DrawImage(gi, 0, 0); }
		void tim_Tick(object sender, EventArgs e) { Calc(); _tfr++; }

		#endregion Events
		#region Calc
		public void Calc() {
			st = DateTime.Now;
			gb.Clear(Color.Black);

			//if(wells.Count > 0) wells[0].x = 0;// (int)_tfr % fx;

			Draw();
		}

		#endregion Calc
		#region Draw
		public Pen[] p = { Pens.Red, Pens.Orange, Pens.Yellow, Pens.Green, Pens.Blue, Pens.Purple, Pens.White, Pens.White, Pens.White, Pens.White };
		public Pen[] tp = { Pens.Red, Pens.Orange, Pens.Yellow, Pens.Green, Pens.SkyBlue, Pens.Violet, Pens.White, Pens.White, Pens.White, Pens.Gray };
		public void Draw() {

			ft = DateTime.Now - st;
			gb.DrawString(ft.TotalMilliseconds.ToString() + "ms", Font, Brushes.White, 0, 0);
			gb.DrawString((1000 / ft.TotalMilliseconds).ToString() + " FPS", Font, Brushes.White, 0, 16);
			gb.DrawString("(" + mouse.X.ToString() + ", " + (fy2 - mouse.Y).ToString() + ") Mouse", Font, Brushes.White, 0, 32);
			gb.DrawString(wells.Count.ToString() + " Wells", Font, Brushes.White, 0, 48);
			gf.DrawImage(gi, 0, 0);
		}

		#endregion Draw
		#region Methods

		#endregion Methods


		//TODO: rectangular map of charges
		//todo equipotential map of charges



	}
}
