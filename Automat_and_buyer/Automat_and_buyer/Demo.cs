using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat_and_buyer
{
    public class Demo
    {
        //Демонстарция эмуляции отношения покупателя и торгового автомата
        public Demo(uint custMoney, uint vendingMoney)
        {
            c = new Customer(custMoney);
            vm = new VendingMachine(vendingMoney);
            c.setVendingMachine(vm);//Покупатель сразу подходит к автомату
            foodPrice = vm.getPrice();//Для демо
            setProtocol();//Установка протокола
            step = 0;
        }

        private ICustomer c;//Покупатель
        private IVendingMachine vm;//Автомат
        private Dictionary<string, uint> foodPrice;//Информация о меню для демонстрационного выбора
        private Dictionary<int, string> protocol;//Протокол результатов
        private uint step;//Итерация покупок

        //Установка протокола
        private void setProtocol()
        {
            protocol = new Dictionary<int, string>();

            protocol.Add(0, "Покупатель получил еду и сдачу\n");

            protocol.Add(1, "Покупатель получил еду, автомат сдачу не выдал\n");
            protocol.Add(2, "Покупателю не хватило денег на еду\n");
            protocol.Add(3, "В автомате никогда не было такой еды\n");
            protocol.Add(4, "Покупатель не подошел к автомату\n");

            protocol.Add(-1, "В депозите недостаточно денег\n");
            protocol.Add(-2, "Выбранная еда закончилась\n");
            protocol.Add(-3, "В автомате никогда не было такой еды\n");

            protocol.Add(-10, "Undefined behavior\n");
        }

        //Генерация разделителя
        private string getHead()
        {
            return String.Format("----------Ход {0}----------", step);
        }

        //Вывод финансов и депозита
        private string getCash()
        {
            string tmp = "";
            tmp += "У покупателя в кошельке:\n";
            tmp += c.showWallet();
            tmp += "В автомате:\n";
            tmp += vm.showWallet();
            return tmp;
        }

        //Вывод меню
        private string getMenu()
        {
            string tmp = "Меню автомата:\n";
            foreach (var f in foodPrice)
            {
                tmp += String.Format(" * {0}  Цена: {1}\n", f.Key, f.Value);
            }
            tmp += "Введите название продукта, который вы хотите купить:";
            return tmp;
        }

        //Демонстрация
        public void startDemonstration()
        {
            string choose;
            while (true)
            {
                Console.WriteLine(getHead());
                Console.WriteLine(getCash());
                Console.WriteLine(c.showFood());
                Console.WriteLine(vm.showFood());
                Console.WriteLine(getMenu());
                choose = Console.ReadLine();
                int i = c.buyFood(choose);
                if (!(protocol.ContainsKey(i))) i = -10;
                Console.WriteLine(protocol[i]);
                step++;
            }
        }
    }
}
