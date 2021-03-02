using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;

namespace Web_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory() { Uri = new Uri("amqp://admin:iamadmin@localhost:5672") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs",
                    type: "topic");

                string createRoutingKey = "";

                if (Btn_Book.Checked)
                {
                    createRoutingKey = "tour.booked";
                }

                if(Btn_Cancel.Checked)
                {
                    createRoutingKey = "tour.cancelled";
                }

                if (!string.IsNullOrEmpty(createRoutingKey))
                {
                    var routingKey = (createRoutingKey.Length > 0) ? createRoutingKey : "anonymous.info";
                    string[] tourStatus = createRoutingKey.Split('.');
                    var message =  $"{textBox_Name.Text}, {textBox_Email.Text} {tourStatus[1]} a tour to {comboBox1.SelectedItem}";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_logs",
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                }
            }
        }
    }
}
