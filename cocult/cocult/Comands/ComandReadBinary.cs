using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cocult.Comands
{
    class ComandReadBinary : IComand
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
        /// <param name="paths">список сохраненных файлов</param>
        public ComandReadBinary(List<string> paths, ListFigure<Figure> _listEnteredShapes)
        {
            NameComand = "читать_бинар";
            this._paths = paths;
            this._listEnteredShapes = _listEnteredShapes;
        }

        /// <summary>
        /// команда для вывода сохраненных фигур в бинарном файле
        /// </summary>
        /// <param name="data">путь файла</param>
        public void Execute(string data)
        {
            Console.Clear();
            _listEnteredShapes.Clear();
            try
            {
                byte[] bytes = File.ReadAllBytes(data);
                    
                string str = CreateStrFromByte(bytes);
                string[] str2 = str.Split("!");
                foreach (var el in str2)
                {
                    string[] figureAndPar = el.Split("$");

                    string figure = Regex.Replace(figureAndPar[0], @"\s+", "");
                    string par = figureAndPar[1];

                    CreateFigure(figure, par);
                }
                
                Console.WriteLine($"Файл загружен");
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Не корректный файл {ex}");
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("не правильно введен файл или такого не существует");
            }
        }

        /// <summary>
        /// метод переводит байты в строку
        /// </summary>
        /// <param name="bytes">полный список байтов из файла</param>
        /// <returns>строку</returns>
        private string CreateStrFromByte(byte[] bytes)
        {
            string str = "";
            for (int i = 0; i < bytes.Length - 3; i++)
            {
                byte[] bytes1 = new byte[2];
                bytes1[0] = bytes[i];
                bytes1[1] = bytes[i + 1];

                byte[] bytes2 = new byte[4];
                bytes2[0] = bytes[i];
                bytes2[1] = bytes[i + 1];
                bytes2[2] = bytes[i + 2];
                bytes2[3] = bytes[i + 3];

                if (char.IsLetterOrDigit(BitConverter.ToChar(bytes1)) || BitConverter.ToChar(bytes1) == '$' || BitConverter.ToChar(bytes1) == '!')
                {
                    str += " ";
                    str += BitConverter.ToChar(bytes1);
                    i += 1;
                }
                else
                {
                    str += " ";
                    int par = BitConverter.ToInt32(bytes2);
                    str += par;
                    i += 3;
                }
            }
            return str;
        }

        /// <summary>
        /// метод для создания обьектов figure из строки
        /// </summary>
        /// <param name="figur">название фигуры</param>
        /// <param name="parametrs">параметры фигуры</param>
        private void CreateFigure(string figur, string parametrs)
        {
            if (figur.ToLower() == Circle.name) _listEnteredShapes.Add(new Circle(App.ToParametrs(parametrs)));
            if (figur.ToLower() == Polygon.name) _listEnteredShapes.Add(new Polygon(App.ToParametrs(parametrs)));
            if (figur.ToLower() == Triangle.name) _listEnteredShapes.Add(new Triangle(App.ToParametrs(parametrs)));
            if (figur.ToLower() == Square.name) _listEnteredShapes.Add(new Square(App.ToParametrs(parametrs)));
            if (figur.ToLower() == Rectangle.name) _listEnteredShapes.Add(new Rectangle(App.ToParametrs(parametrs)));
        }
    }
}
