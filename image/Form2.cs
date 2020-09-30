using ServiceStack.Text.FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        //CSVから取得したデータ
        private List<BattleDate> oriBattleDates = new List<BattleDate>();
        //CSVから取得したモンスターデータ
        private Dictionary<string, string> monsterDic = new Dictionary<string, string>();
        #region イベント
        private void Form2_Load(object sender, EventArgs e)
        {
            setDictionary();
            setBattleDate();
            setCombo();
            showData();

        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

            showData();
        }

        private void cboRank_SelectedIndexChanged(object sender, EventArgs e)
        {

            showData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            showData();

        }
        private void cboLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            showData();
        }

        private void cboResult_SelectedIndexChanged(object sender, EventArgs e)
        {

            showData();
        }

        private void cboHour_SelectedIndexChanged(object sender, EventArgs e)
        {

            showData();
        }

        private void cboSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            showData();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showData();
        }
        #endregion

        #region メソッド(ファイル読み込み)
        private void setBattleDate()
        {
            int index = 1;
            StreamReader sr;
            try
            {
                // 読み込みたいCSVファイルのパスを指定して開く
                sr = new StreamReader(@"date.csv", Encoding.GetEncoding("shift_jis"));
                // 1行目を飛ばす
                sr.ReadLine();
                // 末尾まで繰り返す
                while (!sr.EndOfStream)
                {
                    // CSVファイルの一行を読み込む
                    string line = sr.ReadLine();
                    // 読み込んだ一行をカンマ毎に分けて配列に格納する
                    string[] values = line.Split(',');
                    //形式が正しくない場合飛ばす
                    if (values.Length != 9)
                    {
                        continue;
                    }
                    index += 1;
                    DateTime date = DateTime.Parse(values[0] + values[4]);
                    // 配列からリストに格納する
                    BattleDate battleDate = new BattleDate(date, values[1], values[2], values[3], values[5], values[6], values[7], values[8]);

                    oriBattleDates.Add(battleDate);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("ファイルが存在しません");
            }
            catch
            {
                MessageBox.Show($"ファイルのデータ形式が正しくありません({index}行目)");
            }
        }
        private void setDictionary()
        {
            StreamReader sr;
            try
            {
                // 読み込みたいCSVファイルのパスを指定して開く
                sr = new StreamReader(@"pokemon.csv", Encoding.GetEncoding("shift_jis"));
                {
                    // 1行目を飛ばす
                    sr.ReadLine();
                    // 末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // CSVファイルの一行を読み込む
                        string line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        string[] values = line.Split(',');
                        // 配列からリストに格納する
                        monsterDic.Add(values[2], values[1]);

                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("ファイルが存在しません");
            }
        }



        #endregion




        #region メソッド（コンボにセット）
        private void setCombo()
        {
            List<string> listRank = new List<string>();
            for (int i = 1; i <= 10; i++)
            {

                string str;
                str = Convert.ToString(i);
                listRank.Add(str);
            }
            listRank.Insert(0, "");
            cboRank.DataSource = listRank.ToArray();

            List<string> listLeague = new List<string>();
            listLeague.Add("");
            listLeague.Add("スーパー");
            listLeague.Add("ハイパー");
            listLeague.Add("ハイパープレミア");
            listLeague.Add("マスター");
            listLeague.Add("マスタープレミア");
            cboLeague.DataSource = listLeague.ToArray();

            List<string> listResule = new List<string>();
            listResule.Add("");
            listResule.Add("○");
            listResule.Add("●");
            cboResult.DataSource = listResule.ToArray();

            List<string> listHour = new List<string>();
            for (int i = 0; i <= 23; i++)
            {

                string str;
                str = Convert.ToString(i);
                listHour.Add(str);
            }
            listHour.Insert(0, "");
            cboHour.DataSource = listHour.ToArray();

            List<string> listSeason = new List<string>();
            for (int i = 1; i <= 6; i++)
            {

                string str;
                str = Convert.ToString(i);
                listSeason.Add(str);
            }
            listSeason.Insert(0, "");
            cboSeason.DataSource = listSeason.ToArray();
        }

        #endregion

        #region メソッド（集計）
        private void showData()
        {
            dataGridView3.Rows.Clear();
            dataGridView3.RowTemplate.Height = 60;
            //コンボ未設定の場合、処理終了
            if (cboLeague.DataSource == null || cboResult.DataSource == null || cboHour.DataSource == null || cboSeason.DataSource == null )
            {
                return;
            }
            //開始日取得
            DateTime? fromDate = null;
            if (dtpFrom.Value != null)
            {
                fromDate = dtpFrom.Value.Value.Date;
            }
            //終了日取得
            DateTime? toDate = DateTime.MaxValue;
            if (dtpTo.Value != null)
            {
                toDate = dtpTo.Value.Value.Date;
            }

            //各種条件による抽出
            var battleDates = oriBattleDates
                .Where(d => d.DateTime.Date.CompareTo(fromDate) >= 0 && d.DateTime.Date.CompareTo(toDate) <= 0
                && (cboRank.SelectedValue.ToString() == "" ? true : cboRank.SelectedValue.ToString() == d.Rank.ToString())
                && (cboLeague.SelectedValue.ToString() == "" ? true : cboLeague.SelectedValue.ToString() == d.League.ToString())
                && (cboResult.SelectedValue.ToString() == "" ? true : cboResult.SelectedValue.ToString() == d.Result.ToString())
                && (cboHour.SelectedValue.ToString() == "" ? true : cboHour.SelectedValue.ToString() == d.DateTime.Hour.ToString())
                && (cboSeason.SelectedValue.ToString() == "" ? true : cboSeason.SelectedValue.ToString() == d.Season.ToString())
                )
                .ToList();

            //一体目抽出
            var firstMonster = battleDates
                .GroupBy(g => new { g.Monster1,g.Result })
                .Select(s => new { s.Key.Monster1, s.Key.Result, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();
            //二体目抽出
            var secondMonster = battleDates
                .GroupBy(g => new { g.Monster2, g.Result })
                .Select(s => new { Monster1 = s.Key.Monster2, s.Key.Result, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();
            //三体目抽出
            var thirdMonster = battleDates
                .GroupBy(g => new { g.Monster3, g.Result })
                .Select(s => new { Monster1 = s.Key.Monster3, s.Key.Result, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();

            //一体目から三体目までを合算
            var allList = firstMonster.Concat(secondMonster).Concat(thirdMonster)
                .GroupBy(g => new { g.Monster1, g.Result })
                .Select(s => new { s.Key.Monster1, count = s.Sum(sum => sum.count)
                ,winCount = s.Sum(sum => sum.Result == "○" ? sum.count : 0)
                ,loseCount = s.Sum(sum => sum.Result == "●" ? sum.count : 0 )})
                .GroupBy(g => new { g.Monster1})
                .Select(s => new { s.Key.Monster1,  count = s.Sum(sum => sum.count), winCount = s.Sum(sum => sum.winCount), loseCount = s.Sum(sum => sum.loseCount) })
                .OrderByDescending(o => o.count)
                .ToList();


            //合算分のデータと一体目のみのデータを結合
            var joinList = allList.GroupJoin(firstMonster.GroupBy(g => new { g.Monster1 }).Select(s => new { s.Key.Monster1, count = s.Count() }), all => all.Monster1, first => first.Monster1, (all, tmpFirst) => new { all, tmpFirst })
                            .SelectMany(x => x.tmpFirst.DefaultIfEmpty(), (x, first) => new
                            {
                                x.all.Monster1,
                                TotalCount = x.all.count,
                                TotalRate = ((double)x.all.count * 100 / battleDates.Count()).ToString("0.0"),
                                FirstCount = (first != null) ? first.count : 0,
                                FirstRate = (first != null) ? ((double)first.count * 100 / battleDates.Count()).ToString("0.0") : "0.0",
                                //同一選出されたモンスターをリストに格納する
                                ColleagueList = setColleagueList(battleDates.Where(w => w.Monster1 == x.all.Monster1 || w.Monster2 == x.all.Monster1 || w.Monster3 == x.all.Monster1).ToList(), x.all.Monster1),
                                x.all.winCount,
                                x.all.loseCount
                            }).ToList();

            //順位
            int count = 0;
            //明細に表示
            for (int i = 0; i < joinList.Count; i++)
            {
                var monster = joinList[i];
                //'('で改行
                var name = monster.Monster1.Replace("(", "\r\n(");
                Image image0 = null;
                //同時選出されているモンスターの画像を格納
                List<Image> colImages = new List<Image>();
                if (!string.IsNullOrEmpty(monster.Monster1) && monsterDic.ContainsValue(monster.Monster1))
                {
                    var image = monsterDic.FirstOrDefault(c => c.Value.Equals(monster.Monster1)).Key;
                    image0 = Image.FromFile($@"pokemon/regular/{image}.png");
                }
                monster.ColleagueList
                    .Select((Value, Index) => new { Value, Index }).ToList()
                    .ForEach(f =>
                    {
                        var image = monsterDic.FirstOrDefault(c => c.Value.Equals(monster.ColleagueList[f.Index].Monster1)).Key;
                        if (image != null)
                        {
                            colImages.Add(Image.FromFile($@"pokemon/regular/{image}.png"));
                        }
                    });

                //明細に出力
                if (!string.IsNullOrEmpty(monster.Monster1))
                {
                    count = count + 1;
                    dataGridView3.Rows.Add(count, image0, name, monster.TotalCount, monster.TotalRate + "%", monster.FirstCount, monster.FirstRate + "%",
                        colImages.Count > 0 ? colImages[0] : null, colImages.Count > 1 ? colImages[1] : null, colImages.Count > 2 ? colImages[2] : null
                        , colImages.Count > 3 ? colImages[3] : null,monster.winCount,monster.loseCount,monster.winCount-monster.loseCount);

                }
            }

        }
        private List<Colleague> setColleagueList(List<BattleDate> colleagueList, string own)
        {
            //各種、選択順に応じて集計を行う
            var first = colleagueList
                .GroupBy(g => new { g.Monster1 })
                .Select(s => new { s.Key.Monster1, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();

            var second = colleagueList
                .GroupBy(g => new { g.Monster2 })
                .Select(s => new { Monster1 = s.Key.Monster2, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();
            var third = colleagueList
                .GroupBy(g => new { g.Monster3 })
                .Select(s => new { Monster1 = s.Key.Monster3, count = s.Count() })
                .OrderByDescending(o => o.count)
                .ToList();
            //すべて合算し、集計する
            var all = first.Concat(second).Concat(third)
                .Where(w => w.Monster1 != own)
                .GroupBy(g => new { g.Monster1 })
                .Select(s => new Colleague(s.Key.Monster1, s.Sum(sum => sum.count)))
                .OrderByDescending(o => o.Amount)
                .ToList();

            return all;
        }


        #endregion




    }
}
