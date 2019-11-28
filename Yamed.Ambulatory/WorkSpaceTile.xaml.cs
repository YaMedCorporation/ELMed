using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Yamed.Control;
using Yamed.Server;
using Yamed.Emr;
using Yamed.Registry;
using System.Data.SqlClient;
using Yamed.Entity;
using DevExpress.Xpf.Grid;

namespace Yamed.Ambulatory
{
    /// <summary>
    /// Логика взаимодействия для WorkSpaceTile.xaml
    /// </summary>
    public partial class WorkSpaceTile : UserControl
    {
        public WorkSpaceTile()
        {
            InitializeComponent();
        }

        private void Tile_OnClick(object sender, EventArgs e)
        {
            SqlReader.Select("Select * from Yamed_View_Ophthalmologist", SprClass.LocalConnectionString);
        }
        private D3_SCHET_OMS _sc;
        private GridControl _gc;
        
        private void Tile_Click(object sender, EventArgs e)
        {
            int z_id=0;
            var constr = SprClass.LocalConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comm1 = new SqlCommand($@"select top(1) id from d3_zsl_oms 
where d3_pid=(select pid from yamedregistry where id={RegReg.reg_id}) and DATE_Z_2 is null order by id desc", con);
            con.Open();
            SqlDataReader reader = comm1.ExecuteReader();
            if (reader.HasRows == false)
            {
                z_id=0;
            }

            while (reader.Read()) // построчно считываем данные
            {
                object _zid = reader["ID"];
                z_id = Convert.ToInt32(_zid);
            }
            reader.Close();
            con.Close();
            var zslTempl = new SluchTemplateD31(_gc);
            if (z_id !=0 )
            {
                
                zslTempl.BindSluch(z_id,_sc);
            }
            else
            {
                
                zslTempl.BindEmptySluch2(_sc);
            }
            //int pppid=
            //_sc = ;
            
            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                
                Header = "Случай поликлиники",
                MyControl = zslTempl,
                IsCloseable = "True"
            });
        }
        private void reg_open(object sender, EventArgs e)
        {
            var constr = SprClass.LocalConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comm1 = new SqlCommand($@"update  yamedregistry set pid=(select pid from yamedregistry where id={RegReg.reg_id}),
PacientName=(select PacientName from yamedregistry where id={RegReg.reg_id})
where id=(select min(id) from YamedRegistry where CAST(BeginTime as DATE)='{WorkSpaceControl.wdedit}'  
and pid is null and Reserve=0) and did={WorkSpaceControl.docid} ", con);
            con.Open();
            comm1.ExecuteNonQuery();
            con.Close();

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
            {
                Header = "Регистратура",
                MyControl = new ScheduleControl(),
                IsCloseable = "True",
                TabLocalMenu = new Yamed.Registry.RegistryMenu().MenuElements
               
            });

            
        }
        private int? p_id;
        private void emk_click(object sender, EventArgs e)
        {
            
            var constr = SprClass.LocalConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comm1 = new SqlCommand($@"select pid from YamedRegistry where id={RegReg.reg_id}", con);
            con.Open();
            p_id = (int?)comm1.ExecuteScalar();
            con.Close();

            СommonСomponents.DxTabControlSource.TabElements.Add(new TabElement()
                {
                    Header = "ЭМК Пациента",
                    MyControl = new ClinicEmrPacient(p_id),
                    IsCloseable = "True"
                });
            
        }
    }
}
