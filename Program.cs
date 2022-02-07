using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
	class Program
	{
		static void Main(string[] args)
		{
			World world = new World();
			Snake snake;
			Levels levels;
			
			Console.WindowWidth=130;
			Console.WindowHeight = 40;

			Welcome();

			//Игра
			while(!world.Exit)
			{
				//Инициализация мира
				world = new World();
				snake = new Snake();	
				levels = new Levels();

				//Запускаем ввод в отдельный поток
				Thread inputThread = new Thread(new ThreadStart(world.Input));
				inputThread.Start();

				//Отрисовать стены
				world.DrawWalls();

				//Пока жив играй
				while(!world.gameOver && !world.isWon)
				{
					//Ввод
					world.Input();

					//Логика игры
					world.Logic(snake, levels);

					//Отрисовать игровое поле
					world.Draw();
				}

				//Если выиграл - поздравить
				if(world.isWon)
				{
					Console.Clear();
					Console.WindowWidth = 130;
					Console.WriteLine("\n\n\n\n\n\n\t\t\t\t\t\t\tYou Won!");
					Thread.Sleep(2000);
				}

				//Если вышел (нажата кнопка Х) то выйти
				if(world.Exit)
				{
					break;
				}

				Console.Clear();
				Console.WindowWidth = 130;
				Console.WriteLine("\n\n\n\n\n\n\t\t\t\t\tPress 'R' to Reset the game or 'X' to exit");

				string key;
				//Выйти или Сброс
				do
				{
					key = Console.ReadKey(true).Key.ToString();
					switch(key)
					{
						//Сброс
						case "R":
						{
							Console.Clear();
							Console.WriteLine("\n\n\n\n\n\n\t\t\t\t\t\t\tGood Luck!");
							Thread.Sleep(1000);
							break;
						}
						//Выйти
						case "X":
						{
							world.Exit = true;
							break;
						}
					}
				} while(key != "R" && key !=  "X");
			}
			
		}

		//Преветствие
		static void Welcome(){
			Console.WriteLine(	"\n\n\n\t\t\t\t\t\t\t     Snake" +
								"\n\n\n\t\t\t\t\t\t\t Controls - WASD" +
								"\n\n\n\t\t\t\t\t\t\t   'X' - Exit" +
								"\n\n\n\t\t\t\t\t\t\t  'P' - Pause" +
								"\n\n\n\n\n\n\n\t\t\t\t\t\tPress 'Enter' to start the game");
			Console.ReadLine();
			Console.Clear();
			Console.WriteLine("\n\n\n\n\n\n\t\t\t\t\t\t\tGood Luck!");

			Thread.Sleep(2000);
		}
	}
}
