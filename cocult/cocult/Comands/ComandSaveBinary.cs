using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cocult.Comands
{
    class ComandSaveBinary : IComand
    {
        /// <summary>
        /// список со всеми сохраненными файлами
        /// </summary>
        private List<string> _paths;

        /// <summary>
        /// список для хранения фигур
        /// </summary>
        ListFigure<Figure> _listEnteredShapes;

        /// <summary>
        /// название команды
        /// </summary>
        public string NameComand { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="paths">список со всеми сохраненными файлами</param>
        public ComandSaveBinary(List<string> paths, ListFigure<Figure> _listEnteredShapes)
        {
            NameComand = "сохранить_бинар";
            this._paths = paths;
            this._listEnteredShapes = _listEnteredShapes;
        }

        /// <summary>
        /// команда для сохранения фигур в бинарном файле
        /// </summary>
        /// <param name="data">путь файла</param>
        public void Execute(string data)
        {
            try
            {
                Console.Clear();
                using (FileStream fs = new FileStream(data, FileMode.Append))
                using (BinaryWriter write = new BinaryWriter(fs))
                {
                    foreach (var el in _listEnteredShapes)
                    {
                        string[] str = el.ToString().Split(" ",2).ToArray();

                        SaveString(write, str[0]);
                        write.Write(BitConverter.GetBytes('$'));

                        SaveInt32(write, str[1]);
                        write.Write(BitConverter.GetBytes('!'));
                    }
                }
                _listEnteredShapes.Clear();
                    Console.Clear();
                    Console.WriteLine("Данные сохранены");
                
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Не корректный файл {ex}");
            }
            catch(Exception e)
            {
                Console.Clear();
                Console.WriteLine($"В это место нельзя сохранить файл или этот путь недостижим {e}");
            }
        }

        /// <summary>
        /// метод для сохранения строки
        /// </summary>
        /// <param name="write">класс для работы с бинарным файлом</param>
        /// <param name="str">строка которую переносим</param>
        private void SaveString(BinaryWriter write,string str)
        {
                char[] figure = str.ToCharArray();            
                foreach (var r in figure) write.Write(BitConverter.GetBytes(r));                     
        }

        /// <summary>
        /// метод для сохранения цифры
        /// </summary>
        /// <param name="write">класс для работы с бинарным файлом</param>
        /// <param name="str">сцифра которую переносим</param>
        private void SaveInt32(BinaryWriter write, string str)
        {
                List<int> list = App.ToParametrs(str);
                foreach (int r in list) write.Write(BitConverter.GetBytes(r));           
        }
    }
}

