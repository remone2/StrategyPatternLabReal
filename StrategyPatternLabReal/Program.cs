using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternLabReal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sAttr;
            sAttr = ConfigurationManager.AppSettings.Get("Fee");

            var fileName = @"C:\Users\nisep\source\repos\course9\StrategyPatternLabReal\StrategyPatternLabReal\Orders.txt";
            FileStream fs = File.OpenRead(fileName);
            using (StreamReader sr = new StreamReader(fs))
            {
                string line;
                string[] readValues = new string[4];

                while ((line = sr.ReadLine()) != null)
                {
                    if (readValues[0] != null)
                        Array.Clear(readValues, 0, 4);

                    readValues = line.Split(' ');

                    if (int.Parse(readValues[1]) == 12)
                    {
                        TrainTicket ticket = new TrainTicket(new DecemberFee());
                        ticket.Fee = double.Parse(sAttr);
                        Console.WriteLine($"ticket bought in december, the fee is {ticket.Fee} and you get a discount of {ticket.Discount}. Your total is {ticket.CalculateFee()}");
                    }
                    else if (int.Parse(readValues[1]) == 6 || int.Parse(readValues[1]) == 7)
                    {
                        TrainTicket ticket = new TrainTicket(new JuneJulyFee());
                        ticket.Fee = double.Parse(sAttr);
                        Console.WriteLine($"ticket bought in december, the fee is {ticket.Fee} and you get a discount of {ticket.Discount}. Your total is {ticket.CalculateFee()}");
                    }
                    else
                    {
                        TrainTicket ticket = new TrainTicket(new OtherMonthsFee());
                        ticket.Fee = double.Parse(sAttr);
                        Console.WriteLine($"ticket bought in december, the fee is {ticket.Fee} and you get a discount of {ticket.Discount}. Your total is {ticket.CalculateFee()}");
                    }
                }
            }
            Console.ReadLine();
        }
    }

    public class TrainTicket
    {
        public double Fee { get; set; }
        public double Discount { get; set; } = 0;
        public CalculateFeeBehaviour Cfb { get; set; }

        public TrainTicket(CalculateFeeBehaviour cfb)
        {
            Cfb = cfb;
        }

        public double CalculateFee()
        {
            return Cfb.CalculateFee(Fee);
        }
    }

    public interface CalculateFeeBehaviour
    {
        double CalculateFee(double fee);
    }

    public class DecemberFee : CalculateFeeBehaviour
    {
        public double CalculateFee(double fee)
        {
            return fee * 2;
        }
    }

    public class JuneJulyFee : CalculateFeeBehaviour
    {
        public double Discount = 0.25;

        public double CalculateFee(double fee)
        {
            return fee - (fee * Discount);
        }
    }

    public class OtherMonthsFee : CalculateFeeBehaviour
    {
        public double CalculateFee(double fee)
        {
            return fee;
        }
    }
}
