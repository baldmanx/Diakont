using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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

namespace Diakont
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  Функция добавления должности
        public void AddJob(DiakontEntities de, ComboBox cmb, TextBox tb, DatePicker dp)
        {
            var job = new Jobs();
            var jobs = de.Jobs.ToList();

            //  Формирование должности, исходя и введённых данных 
            job.job_id = System.Guid.NewGuid(); 
            job.job_name = cmb.SelectedItem.ToString();
            job.jdate_from = dp.SelectedDate;
            job.fee = decimal.Parse(tb.Text);

            de.Jobs.Add(job);

            de.Jobs.Load();
            de.SaveChanges();
        }

        /*public void ComputeReport(DiakontEntities de, ComboBox d, DatePicker date_from, DatePicker date_to)
        {
            int fee = 0;
            int workers_amount = 0;
            int payment = 0;
            int index_j = 0;
            int index_d = 0;
           
            var report = new Reports();

            var jobs = de.Jobs.ToList();
            var departments = de.Departments.ToList();
            var reports = de.Reports.ToList();

            List<DateTime> dates_list = new List<DateTime>();
            Dictionary<DateTime, DateTime> dates_ranges = new Dictionary<DateTime, DateTime>();

            foreach (var job in jobs)
            {
                if (job.jdate_from >= date_from.SelectedDate && job.jdate_from <= date_to.SelectedDate)
                {
                    dates_list.Add(job.jdate_from.Value);
                }
            }

            foreach (var dep in departments)
            {
                if (dep.ddate_from >= date_from.SelectedDate && dep.ddate_from <= date_to.SelectedDate)
                {
                    dates_list.Add(dep.ddate_from.Value);
                }
            }

            dates_list.Sort();
            dates_list = dates_list.Distinct().ToList();

            for (int i = 0; i < dates_list.LongCount() - 1; i++)
            {
                dates_ranges.Add(dates_list[i], dates_list[i + 1]);
            }

            for (int i = 0; i < dates_ranges.LongCount(); i++)
            {
                for (int j = 0; j < jobs.LongCount(); j++)
                {
                    if (dates_ranges.Keys.ElementAt(i) == jobs[j].jdate_from)
                    {
                        fee = (int)jobs[j].fee;
                        index_j = j;
                        break;
                    }
                }


                for (int j = 0; j < departments.LongCount(); j++)
                {
                    if (dates_ranges.Values.ElementAt(i) == departments[j].ddate_from)
                    {
                        workers_amount = (int)departments[j].workers_amount;
                        index_d = j;
                        break;
                    }
                }

                payment = fee * workers_amount;

                report.report_id = System.Guid.NewGuid();
                report.j_id = jobs[0].job_id;
                report.d_id = departments[0].department_id;
                report.date_from = dates_ranges.Keys.ElementAt(i);
                report.date_to = dates_ranges.Values.ElementAt(i);
                report.monthly_pay = (decimal)payment;

                reports.Add(report);

                de.Reports.Load();
                de.SaveChanges();
            }
        }*/

        //  Функция добавления отдела(аналогична добавлению должности)
        public void AddDepartment(DiakontEntities de, ComboBox dcmb, ComboBox jcmb, TextBox tb, ComboBox ddcmb)
        {
            var job = new Jobs();
            var jobs = de.Jobs.ToList();

            var department = new Departments();
            var departments = de.Departments.ToList();

            department.department_id = System.Guid.NewGuid();
            department.department_name = dcmb.SelectedItem.ToString();
            job.job_name = jcmb.SelectedItem.ToString();
            department.ddate_from = (DateTime)ddcmb.SelectedItem;
            department.workers_amount = int.Parse(tb.Text);

            de.Departments.Add(department);

            de.Departments.Load();
            de.SaveChanges();
        }

        DiakontEntities context;
        List<int> months = new List<int>(); // Список разниц дат по месяцам в определённом диапозоне
        List<DateTime> dates_list = new List<DateTime>(); // Список дат для формирования отчёта

        public MainWindow()
        {
            InitializeComponent();

            context = new DiakontEntities();
            context.Jobs.Load();

            // Заполнение и отображение содержания элементов выпадающих списков по умолчанию
            var init_jobs = context.Jobs.ToList();
            var init_departments = context.Departments.ToList();

            jobComboBox.Items.Add(init_jobs[(int)(init_jobs.LongCount() - 2)].job_name);
            jobComboBox.Items.Add(init_jobs[(int)(init_jobs.LongCount() - 1)].job_name);

            jComboBox.Items.Add(init_jobs[(int)(init_jobs.LongCount() - 1)].job_name);

            jobComboBox.SelectedItem = jobComboBox.Items[0];

            departmentComboBox.Items.Add(init_departments[(int)(init_departments.LongCount() - 2)].department_name);
            departmentComboBox.Items.Add(init_departments[(int)(init_departments.LongCount() - 1)].department_name);

            reportComboBox.Items.Add(init_departments[(int)(init_departments.LongCount() - 2)].department_name);
            reportComboBox.Items.Add(init_departments[(int)(init_departments.LongCount() - 1)].department_name);
                       
            jobComboBox.SelectedItem = jobComboBox.Items[0];
            departmentComboBox.SelectedItem = departmentComboBox.Items[0];

            // Заполнение таблиц данными, которые уже были добавлены в БД, если такие имеются
            jobDataGrid.ItemsSource = context.Jobs.SqlQuery("select * from Jobs where fee is not null").ToList();
            departmentDataGrid.ItemsSource = context.Departments.SqlQuery("select * from Departments where workers_amount is not null").ToList();
            reportDataGrid.ItemsSource = context.Reports.SqlQuery("select * from Reports where monthly_pay is not null").ToList();
        }

        private void feeButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавление должности и обновление отображаемых данных
            AddJob(context, jobComboBox, jobTextBox, jobDatePicker);
            jobDataGrid.ItemsSource = context.Jobs.SqlQuery("select * from Jobs where fee is not null").ToList();
        }

        private void departmentButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавление отдела и обновление отображаемых данных
            AddDepartment(context, departmentComboBox, jComboBox, departmentTextBox, ddepartmentComboBox);
            departmentDataGrid.ItemsSource = context.Departments.SqlQuery("select * from Departments where workers_amount is not null").ToList(); 
        }

        private void reportButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавление новой записи в таблицу отчётов, исходя из введённых данных
            reportDataGrid.ItemsSource = context.Reports.SqlQuery("insert into Reports(report_id, j_id, d_id, date_from, date_to, monthly_pay) values(NEWID(), '1C7D8ACE-D916-4B74-AA2B-A044A1E9D0C8', 'B029DB9C-3078-4063-8859-14BFEC6DD9D7', '" + dates_list[dateRangeComboBox.SelectedIndex] + "', '" + dates_list[dateRangeComboBox.SelectedIndex + 1] + "'," +
                                                                  "(select fee from Jobs where jdate_from = '" + dates_list[dateRangeComboBox.SelectedIndex] + "') * parsename(convert(varchar, convert(money, (select workers_amount from Departments where ddate_from = '" + dates_list[dateRangeComboBox.SelectedIndex] + "')), 1), 2) * " + months[dateRangeComboBox.SelectedIndex] + 
                                                                  "); select * from Reports where monthly_pay is not null;").ToList();            
        }

        private void departmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Заполнение содержания выпадающего списка выбора должности на форме отдела

            var init_jobs = context.Jobs.ToList();

            switch (departmentComboBox.SelectedItem.ToString())
            {
                case "КБ":
                    {
                        jComboBox.Items[0] = init_jobs[(int)(init_jobs.LongCount() - 2)].job_name;
                        break;
                    }
                case "ТО":
                    {
                        jComboBox.Items[0] = init_jobs[(int)(init_jobs.LongCount() - 1)].job_name;
                        break;
                    }
            }            
        }

        private void jComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Заполнение содержания выпадающего списка выбора даты с на форме отдела

            var jobs = context.Jobs.ToList();

            switch (jComboBox.SelectedValue.ToString())
            {
                case "Инженер-конструктор":
                    {
                        foreach (var job in jobs)
                        {
                            if (job.job_name == jComboBox.SelectedItem.ToString())
                            {
                                ddepartmentComboBox.Items.Add(job.jdate_from);
                            }
                        }
                        break;
                    }
                case "Инженер-технолог":
                    {
                        foreach (var job in jobs)
                        {
                            if (job.job_name == jComboBox.SelectedItem.ToString())
                            {
                                ddepartmentComboBox.Items.Add(job.jdate_from);
                            }
                        }
                        break;
                    }
            }
        }

        private void reportComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Заполнение содержания выпадающего списка выбора диапазона дат по каждому отделу на форме отчёта
            // Заполнение списка дат по каждому отделу
            // Заполнение списка разниц дат по месяцам в определённом диапозоне

            var departments = context.Departments.ToList();

            switch (reportComboBox.SelectedValue.ToString())
            {
                case "КБ":
                    {
                        foreach (var dep in departments)
                        {
                            if (dep.department_name == reportComboBox.SelectedItem.ToString() && dep.workers_amount != null)
                            {
                                dates_list.Add(dep.ddate_from.Value);
                            }
                        }

                        dates_list.Sort();

                        for (int i = 0; i < dates_list.LongCount() - 1; i++)
                        {
                            months.Add(dates_list[i + 1].Month - dates_list[i].Month);
                            dateRangeComboBox.Items.Add(dates_list[i] + " - " + dates_list[i + 1]);
                        }

                        break;
                    }

                case "ТО":
                    {
                        foreach (var dep in departments)
                        {
                            if (dep.department_name == reportComboBox.SelectedItem.ToString() && dep.workers_amount != null)
                            {
                                dates_list.Add(dep.ddate_from.Value);
                            }
                        }

                        dates_list.Sort();

                        for (int i = 0; i < dates_list.LongCount() - 1; i++)
                        {
                            months.Add(dates_list[i + 1].Month - dates_list[i].Month);
                            dateRangeComboBox.Items.Add(dates_list[i] + " - " + dates_list[i + 1]);
                        }

                        break;
                    }            
            }
        }
    }
}