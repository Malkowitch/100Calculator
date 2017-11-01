using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Views.InputMethods;
using static Android.Views.ViewGroup;
using Android.Graphics;

namespace WarhammerCalculator
{
    [Activity(Label = "Warhammer Calculator", MainLauncher = true, Icon = "@drawable/favicon")]
    public class MainActivity : Activity
    {
        private TextView txtVResult;
        private EditText etxtDiff;
        private EditText etxtRoll;
        private Spinner ddlDiff;
        private RadioGroup rgShooting1;
        private RadioGroup rgShooting2;
        private RadioGroup rgShootMode;
        private int rbChecked;
        private int scrW;
        private int scrH;

        private InputMethodManager imm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Layouts
            LinearLayout layoutbase = FindViewById<LinearLayout>(Resource.Id.mainLinear);
            layoutbase.SetBackgroundColor(Color.DarkRed);
            RelativeLayout relativebase = new RelativeLayout(this);
            relativebase.SetBackgroundColor(Color.Transparent);

            //Gets the actual phones dimensions
            var met = Resources.DisplayMetrics;
            scrW = met.WidthPixels;
            scrH = met.HeightPixels;

            //New colors
            Color tBlack = new Color(0, 0, 0, 80);
            Color etxtHintColor = new Color(56, 56, 56);
            Color etxtFontColor = Color.Silver;

            //Color state list
            Android.Content.Res.ColorStateList cslRG = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] {-Android.Resource.Attribute.StateEnabled},
                    new int[] {Android.Resource.Attribute.StateEnabled}
                },
                new int[] {
                    tBlack,
                    Color.Silver
                    });

            //Item creations
            etxtDiff = new EditText(this)
            {
                InputType = Android.Text.InputTypes.ClassNumber,
                Hint = GetString(Resource.String.difficulty),
                Gravity = GravityFlags.Center
            };
            etxtDiff.SetHintTextColor(etxtHintColor);
            etxtDiff.SetTextColor(etxtFontColor);
            etxtRoll = new EditText(this)
            {
                InputType = Android.Text.InputTypes.ClassNumber,
                Hint = GetString(Resource.String.roll),
                Gravity = GravityFlags.Center
            };
            etxtRoll.SetHintTextColor(etxtHintColor);
            etxtRoll.SetTextColor(etxtFontColor);
            txtVResult = new TextView(this)
            {

                TextAlignment = TextAlignment.Gravity,
                Gravity = GravityFlags.Center,
                TextSize = 20,
                Text = Calculate(etxtDiff.Text, etxtRoll.Text),
                Id = Resource.String.idtxtResult
            };
            txtVResult.SetTextColor(Color.Black);
            TextView txtDiff = new TextView(this)
            {
                Id = 99,
                Gravity = GravityFlags.Left,
                TextSize = 18,
                Text = GetString(Resource.String.diff_text)
            };
            txtDiff.SetTextColor(Color.Black);

            ddlDiff = new Spinner(this)
            {
                Id = 98
            };


            //Radio group
            rgShooting1 = new RadioGroup(this)
            {
                Orientation = Orientation.Horizontal
            };
            RadioButton[] rb1 = new RadioButton[3];
            for (int i = 0; i < rb1.Length; i++)
            {
                rb1[i] = new RadioButton(this)
                {
                    Text = GetString(GetShootingID(i)),
                    Id = 100 + i,
                    ButtonTintList = cslRG
                };
                rb1[i].SetTextColor(Color.Goldenrod);
                rb1[i].Click += RBRangeDeselect_Click;
                rgShooting1.AddView(rb1[i]);
            }
            rgShooting2 = new RadioGroup(this)
            {
                Orientation = Orientation.Horizontal
            };
            RadioButton[] rb2 = new RadioButton[2];
            for (int i = 0; i < rb2.Length; i++)
            {
                rb2[i] = new RadioButton(this)
                {
                    Text = GetString(GetShootingID(i + 3)),
                    Id = 103 + i,
                    ButtonTintList = cslRG
                };
                rb2[i].SetTextColor(Color.Goldenrod);
                rb2[i].Click += RBRangeDeselect_Click;
                rgShooting2.AddView(rb2[i]);
            }
            rgShooting1.ClearCheck();
            rgShooting2.ClearCheck();


            rgShootMode = new RadioGroup(this)
            {
                Orientation = Orientation.Horizontal
            };
            RadioButton[] rbMode = new RadioButton[3];
            for (int i = 0; i < rbMode.Length; i++)
            {
                rbMode[i] = new RadioButton(this)
                {
                    Text = GetString(GetShotModeId(i)),
                    Id = 200 + i,
                    Gravity = GravityFlags.Center,
                    ButtonTintList = cslRG
                };
                rbMode[i].SetTextColor(Color.Goldenrod);
                rgShootMode.AddView(rbMode[i]);
            }
            rgShootMode.ClearCheck();


            //Create layouts
            LinearLayout.LayoutParams llpMW = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            LinearLayout.LayoutParams llpMWBM40 = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            {
                BottomMargin = 40
            };
            LinearLayout.LayoutParams llpMWBM80 = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            {
                BottomMargin = 80
            };
            LinearLayout.LayoutParams llpTxtDiff = new LinearLayout.LayoutParams(MathCeiling(scrW, 0.4), LayoutParams.WrapContent)
            {
                BottomMargin = 40,
                Gravity = GravityFlags.Left
            };
            LinearLayout.LayoutParams llpDdlDiff = new LinearLayout.LayoutParams(MathCeiling(scrW, 0.44), LayoutParams.WrapContent)
            {
                BottomMargin = 40,
                Gravity = GravityFlags.Right
            };
            LinearLayout.LayoutParams llpWWC = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
            {
                Gravity = GravityFlags.Center
            };
            RelativeLayout.LayoutParams rlpRlo = new RelativeLayout.LayoutParams(scrW, scrH);
            RelativeLayout.LayoutParams rlpTxtDiff = new RelativeLayout.LayoutParams(MathCeiling(scrW, 0.4), LayoutParams.WrapContent)
            {
                LeftMargin = MathCeiling(scrW, 0.1),
                TopMargin = 20,
                BottomMargin = 40
            };
            rlpTxtDiff.AddRule(LayoutRules.AlignParentLeft);
            RelativeLayout.LayoutParams rlpDdlDiff = new RelativeLayout.LayoutParams(MathCeiling(scrW, 0.44), LayoutParams.WrapContent)
            {
                TopMargin = 20,
                BottomMargin = 40
            };
            rlpDdlDiff.AddRule(LayoutRules.AlignParentRight);

            //Add layouts
            etxtDiff.LayoutParameters = llpMW;
            ddlDiff.LayoutParameters = rlpDdlDiff;
            txtDiff.LayoutParameters = rlpTxtDiff;
            etxtRoll.LayoutParameters = llpMWBM40;
            txtVResult.LayoutParameters = llpMWBM80;
            rgShooting1.LayoutParameters = llpMW;
            rgShooting2.LayoutParameters = llpWWC;
            rgShootMode.LayoutParameters = llpMW;

            //Drop down items
            var diffArrays = ArrayAdapter.CreateFromResource(this, Resource.Array.difficulty_array, Resource.Layout.spinner_item);
            diffArrays.SetDropDownViewResource(Resource.Layout.spinner_dd_items);
            ddlDiff.Adapter = diffArrays;
            ddlDiff.SetSelection(diffArrays.GetPosition("Challenging +0"));


            //Add methods
            etxtDiff.TextChanged += ResultChanger;
            etxtRoll.TextChanged += ResultChanger;
            ddlDiff.ItemSelected += DdlDiff_ItemSelected;
            rgShooting1.CheckedChange += RgShooting1_CheckedChange;
            rgShooting2.CheckedChange += RgShooting2_CheckedChange;
            rgShootMode.CheckedChange += RgShootMode_CheckedChange;

            //Order of insertion
            layoutbase.AddView(etxtDiff);
            layoutbase.AddView(etxtRoll);
            layoutbase.AddView(txtVResult);
            layoutbase.AddView(AddLine(8000));
            relativebase.AddView(txtDiff);
            relativebase.AddView(ddlDiff);
            layoutbase.AddView(relativebase);
            //layoutbase.AddView(txtDiff);
            //layoutbase.AddView(ddlDiff);
            layoutbase.AddView(AddLine(8001));
            layoutbase.AddView(rgShooting1);
            layoutbase.AddView(rgShooting2);
            layoutbase.AddView(AddLine(8002));
            layoutbase.AddView(rgShootMode);

            //Layout
            layoutbase.Click += MainActivity_Click;
            imm = (InputMethodManager)GetSystemService(InputMethodService);
        }

        private void RBRangeDeselect_Click(object sender, EventArgs e)
        {
            RadioButton nowclicked = (RadioButton)sender;
            if (rbChecked == nowclicked.Id)
                ClearBothRG();
            else
                rbChecked = nowclicked.Id;
        }
        private void ClearBothRG()
        {
            rgShooting1.ClearCheck();
            rgShooting2.ClearCheck();
            rbChecked = 0;
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private View AddLine(int _id)
        {
            View line = new View(this)
            {
                Id = _id
            };
            LinearLayout.LayoutParams llpLine = new LinearLayout.LayoutParams(width: scrW, height: 5);
            line.SetBackgroundColor(Color.DarkGoldenrod);
            line.LayoutParameters = llpLine;
            return line;
        }

        private void RgShootMode_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private void RgShooting2_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (e.CheckedId == -1)
                return;
            rgShooting1.CheckedChange -= RgShooting1_CheckedChange;
            rgShooting1.ClearCheck();
            rgShooting1.CheckedChange += RgShooting1_CheckedChange;
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private void RgShooting1_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (e.CheckedId == -1)
                return;
            rgShooting2.CheckedChange -= RgShooting2_CheckedChange;
            rgShooting2.ClearCheck();
            rgShooting2.CheckedChange += RgShooting2_CheckedChange;
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private void DdlDiff_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private int MathCeiling(int _x, double _y)
        {
            int res = Convert.ToInt32(Math.Ceiling(_x * _y));
            return res;
        }

        private void MainActivity_Click(object sender, EventArgs e)
        {
            imm.HideSoftInputFromWindow(etxtDiff.WindowToken, 0);
            imm.HideSoftInputFromWindow(etxtRoll.WindowToken, 0);
        }

        private void ResultChanger(object sender, Android.Text.TextChangedEventArgs e)
        {
            txtVResult.Text = Calculate(etxtDiff.Text, etxtRoll.Text);
        }

        private string Calculate(string _diff, string _roll)
        {
            string res = GetString(Resource.String.show_result);
            if (!string.IsNullOrEmpty(_diff) && !string.IsNullOrEmpty(_roll))
            {
                var isDiffNum = int.TryParse(_diff, out var n);
                var isRollNum = int.TryParse(_roll, out n);

                if (isDiffNum && isRollNum)
                {
                    int diff = Convert.ToInt32(_diff);
                    int roll = Convert.ToInt32(_roll);
                    diff += DDLSelected(ddlDiff.SelectedItem.ToString());
                    diff += ShootingRange(rgShooting1.CheckedRadioButtonId, rgShooting2.CheckedRadioButtonId);
                    diff += ShootingMode(rgShootMode.CheckedRadioButtonId);
                    if (diff <= 0 && roll == 1)
                    {
                        diff = 1;
                    }

                    double x = diff - roll;
                    double result = x / (double)10;

                    if (result == 0)
                    {
                        res = GetString(Resource.String.result_success) + " 1";
                    }
                    else if (result > 0)
                    {
                        int ceiled = Convert.ToInt32(Math.Ceiling(result));
                        res = GetString(Resource.String.result_success) + " " + ceiled;
                    }
                    else if (result < 0)
                    {
                        int floored = Convert.ToInt32(Math.Floor(result));
                        res = GetString(Resource.String.result_failure) + " " + floored;
                    }
                }
                else
                {
                    res = GetString(Resource.String.result_error);
                }
            }
            return res;
        }

        private int ShootingRange(int _rb1I, int _rb2I)
        {
            if (_rb1I != -1)
                switch (_rb1I)
                {
                    case 100: return 30;
                    case 101: return 10;
                    case 102: return 0;
                }
            if (_rb2I != -1)
                switch (_rb2I)
                {
                    case 103: return -10;
                    case 104: return -30;
                }

            return 0;
        }

        private int ShootingMode(int _rbI)
        {
            switch (_rbI)
            {
                case 200: return 0;
                case 201: return 10;
                case 202: return 20;
                default: return 0;
            }
        }

        private int DDLSelected(string _selected)
        {
            int res = 0;

            switch (_selected)
            {
                case "Trivial +60": res = 60; break;
                case "Elementary +50": res = 50; break;
                case "Simple +40": res = 40; break;
                case "Easy +30": res = 30; break;
                case "Routine +20": res = 20; break;
                case "Ordinary +10": res = 10; break;
                case "Difficult -10": res = -10; break;
                case "Hard -20": res = -20; break;
                case "Very Hard -30": res = -30; break;
                case "Ardous -40": res = -40; break;
                case "Punishing -50": res = -50; break;
                case "Hellish -60": res = -60; break;
                default: break;
            }

            return res;
        }

        private int GetShootingID(int _index)
        {
            switch (_index)
            {
                case 0: return Resource.String.rb_shoot_0;
                case 1: return Resource.String.rb_shoot_1;
                case 2: return Resource.String.rb_shoot_2;
                case 3: return Resource.String.rb_shoot_3;
                case 4: return Resource.String.rb_shoot_4;
                default: return 5;
            }
        }

        private int GetShotModeId(int _index)
        {
            switch (_index)
            {
                case 0: return Resource.String.rb_shotmode_0;
                case 1: return Resource.String.rb_shotmode_1;
                case 2: return Resource.String.rb_shotmode_2;
                default: return 3;
            }
        }
    }
}

