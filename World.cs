using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
	class World
	{
		//Библиотеки------------------------------------------------------------------------------------------------------------------------
		public enum Direction { STOP = 0, LEFT, RIGHT, UP, DOWN }	//Словарь направлений
		Random rand = new Random();

		//Переменные, поля, массивы----------------------------------------------------------------------------------------------------------
		public Direction dir;										//Библиотека движений
		//Массивы
		public const int rows = 25;									//Ряды
		public const int columns = 50;								//Столбы
		public char[,] GameField = new char[rows, columns];         //Игровое поле
																	//Булы
		public bool gameOver, isWon, Exit = false, isFruit = false,        // Конец игры,	Победа,		Выход,		Есть ли фрукт
					isDoubleFruit = false,                          //Есть ли Супер-фрукт
					isSpeed = false;        //Есть ли бонус скорости
											//Байты
		public byte whatSpeed;										//Какая скорость (отнять/прибавить)
		//Числа
		int score;													//Очки
		int Speed = 100;											//Скорость (время на сколько потом усыпить)



		//Функции----------------------------------------------------------------------------------------------------------------------------


		//Инициализация
		public World(){
			Console.Clear();
			Console.CursorVisible = false;
			Console.WindowWidth = 70;
			gameOver = false;
			isWon = false;
			whatSpeed = (byte)rand.Next(2);
			dir = Direction.STOP;
			score = 0;
		}



		//Отрисовка стен (единожды)
		public void DrawWalls(){
			Console.Clear();

			//Отступ сверху
			Console.Write("\n\n\n\t");
			//Отрисовка верхней границы----------------
			Console.Write('╔');
			for(int i = 0; i < columns; i++)
			{
				Console.Write('═');
			}
			Console.Write('╗' + "\n");

			//Отрисовка боковых границ и игрового поля-----------
			for(int i = 0; i < rows; i++)
			{
				//Отступ сбоку
				Console.Write("\t");
				Console.Write('║');         //Левая граница
				Console.SetCursorPosition(59, 4 + i);
				Console.Write('║');         //Правая граница
				Console.WriteLine();
			}

			//Отступ сбоку
			Console.Write("\t");
			
			//Отрисовка нижней границы----------------
			Console.Write('╚');
			for(int i = 0; i < columns; i++)
			{
				Console.Write('═');
			}
			Console.Write('╝');
			Console.WriteLine();


			Console.WriteLine("\n\n\n\tScore : {0}\t\tSpeed = {1}\n\n\n", score, Speed);
		}

		//Отрисовка игрового поля
		public void Draw(){

			//Отрисовка линий
			for(int i = 0; i < rows; i++)
			{
				//В начало линии игрового поля (после левой стенки)
				Console.SetCursorPosition(9, 4 + i);

				//Отрисовка колонок
				for(int j = 0; j < columns; j++)
				{
					//Фрукт - тёмно-зелёный
					if(GameField[i,j] == '$')
					{
						Console.ForegroundColor = ConsoleColor.DarkGreen;
						Console.Write('$');
						Console.ResetColor();
					}
					//Супер-фрукт - красный
					else if(GameField[i, j] == '@')
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write('@');
						Console.ResetColor();
					}
					//Скорость - синяя
					else if(GameField[i, j] == '!')
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write('!');
						Console.ResetColor();
					}
					//Отрисовать клетку
					else
					{
						Console.Write(GameField[i, j]);
					}
				}

			}

			//Скорость игры
			Thread.Sleep(Speed);
		}

		//Ввод
		public void Input(){

			if(Console.KeyAvailable)
			{
				switch(Console.ReadKey(true).Key.ToString())
				{
					//Задание направления---------------
					case "A":
					{
						if(dir == Direction.RIGHT)
						{
							dir = Direction.RIGHT;
							break;
						}
						dir = Direction.LEFT;
						break;
					}
					case "D":
					{
						if(dir == Direction.LEFT)
						{
							dir = Direction.LEFT;
							break;
						}
						dir = Direction.RIGHT;
						break;
					}
					case "W":
					{
						if(dir == Direction.DOWN)
						{
							dir = Direction.DOWN;
							break;
						}
						dir = Direction.UP;
						break;
					}
					case "S":
					{
						if(dir == Direction.UP)
						{
							dir = Direction.UP;
							break;
						}
						dir = Direction.DOWN;
						break;
					}
					//-----------------------------------
					//Выход
					case "X":
					{
						gameOver = true;
						Exit = true;
						break;
					}
					//Пауза
					case "P":
					{
						Console.Clear();
						Console.WriteLine("\n\n\n\n\n\n\t\t\t\tPAUSE");
						Console.WriteLine("\n\n\n\n\n\n\t\t\tPress 'Esc' to continue");
						do
						{

						} while(Console.ReadKey(true).Key != ConsoleKey.Escape);
						Console.Clear();
						DrawWalls();
						break;
					}
				}

			}
		}

		//Логика игры
		public bool Logic(Snake snake, Levels levels){

			int[] prev = { snake.sSnakeTail[0, 0], snake.sSnakeTail[1, 0] };
			//int[] temp = new int[2];
			snake.sSnakeTail[0, 0] = snake.xSnakeHead;
			snake.sSnakeTail[1, 0] = snake.ySnakeHead;
			
			//перемещение головы змеи
			switch(dir)
			{								//перемещение змейки
				case Direction.LEFT:
				{
					snake.ySnakeHead--;
					break;
				}
				case Direction.RIGHT:
				{
					snake.ySnakeHead++;
					break;
				}
				case Direction.UP:
				{
					snake.xSnakeHead--;
					break;
				}
				case Direction.DOWN:
				{
					snake.xSnakeHead++;
					break;
				}
			}



			//Если хвост максимальный = победил
			if(snake.nTail == snake.sSnakeTail.Length / 2)
			{
				return isWon = true;
			}



			//в стенку - умер
			if(snake.ySnakeHead >= columns || snake.ySnakeHead < 0 || snake.xSnakeHead >= rows || snake.xSnakeHead < 0)
			{
				return gameOver = true;
			}

			//в себя или препятствие - умер
			else if(Is_Snake_Head_Is_It('#', snake) || Is_Snake_Head_Is_It('*', snake))
			{
				return gameOver = true;
			}




			/*//в себя - умер
			for(int i = 0; i < snake.nTail; i++)
			{
				if(snake.sSnakeTail[0, i] == snake.xSnakeHead && snake.sSnakeTail[1, i] == snake.ySnakeHead)
					gameOver = true;                //сожрал хвост - умри
			}
			*/



			//Препятствия	----	1е	\	2е
			{
				//Препятствие 1го уровня
				if(score >= levels.lvl_1_Score_Need)
				{
					for(int k = 0; k < levels.lvl_1_Wall.Length / 2; k++)
					{
						//проверка на то пустая ли ячейка что бы заполнить её препятствиями
						if(GameField[levels.lvl_1_Wall[0, k], levels.lvl_1_Wall[1, k]] == ' ')
						{

							GameField[levels.lvl_1_Wall[0, k], levels.lvl_1_Wall[1, k]] = '#';
						}
					}
				}

				//Препятствие 2го уровня
				if(score >= levels.lvl_2_Score_Need)
				{
					for(int k = 0; k < levels.lvl_2_Wall.Length / 2; k++)
					{
						//проверка на то пустая ли ячейка что бы заполнить её препятствиями
						if(GameField[levels.lvl_2_Wall[0, k], levels.lvl_2_Wall[1, k]] == ' ')
						{

							GameField[levels.lvl_2_Wall[0, k], levels.lvl_2_Wall[1, k]] = '#';
						}
					}
				}
			}



			//Фрукт - обычный ----	сьел фрукт	/	фрукт если пустая клетка	/	перегенерация фрукта если не удалось разместить
			{ 
			//Сьел фрукт - +очки, новый фрукт, +скорость
			if(Is_Snake_Head_Is_It('$', snake))
			{
				score += 10;                                //+ очки	
				levels.Generate_Bonus(ref levels.FruitX, ref levels.FruitY);
				snake.nTail++;								//увеличение хвоста
				Speed -= 3;                                 //Увеличение скорости
				isFruit = false;
				Console.SetCursorPosition(16, 33);
				Console.Write(score);
				Console.SetCursorPosition(40, 33);
				Console.Write("    ");
				Console.SetCursorPosition(40, 33);
				Console.Write(Speed);
				}

			//Фрукт если пустая клетка
			if(Is_Empty_Cell(levels.FruitX, levels.FruitY)
				&& !isFruit
				&& Can_Be_Placed_Bonus(levels.FruitX, levels.FruitY)) 
			{
				GameField[levels.FruitX, levels.FruitY] = '$';
				isFruit = true;
			}
			//Перегенерация фрукта если его не удалось разместить
			else if (!isFruit)
			{
				levels.Generate_Bonus(ref levels.FruitX, ref levels.FruitY);
			}
			}

			//Супер-фрукт ----	сьел супер-фрукт	/	супер-фрукт если пустая клетка	/	перегенерация супер-фрукта если не удалось разместить
			{
			//Сьел супер-фрукт - ++очки, новый супер-фрукт, ++скорость
			if(Is_Snake_Head_Is_It('@', snake))
			{
				score += 40;                                //+ очки	
				levels.Generate_Bonus(ref levels.X_DoubleFruit_Bonus, ref levels.Y_DoubleFruit_Bonus);
				snake.nTail ++;                              //увеличение хвоста
				Speed -= 6;                                 //Увеличение скорости
				isDoubleFruit = false;
				Console.SetCursorPosition(16, 33);
				Console.Write(score);
				Console.SetCursorPosition(40, 33);
				Console.Write("    ");
				Console.SetCursorPosition(40, 33);
				Console.Write(Speed);
				}

			//Супер-Фрукт если пустая клетка
			if(Is_Empty_Cell(levels.X_DoubleFruit_Bonus, levels.Y_DoubleFruit_Bonus)
				&& !isDoubleFruit
				&& score >= levels.lvl_1_Score_Need && (score % levels.DoubleFruit_Need_Score) == 0
				&& Can_Be_Placed_Bonus(levels.X_DoubleFruit_Bonus,levels.Y_DoubleFruit_Bonus))
			{
				GameField[levels.X_DoubleFruit_Bonus, levels.Y_DoubleFruit_Bonus] = '@';
				isDoubleFruit = true;
			}

			//Перегенерация супер-фрукта если его не удалось разместить
			else if(!isDoubleFruit)
			{
				levels.Generate_Bonus(ref levels.X_DoubleFruit_Bonus, ref levels.Y_DoubleFruit_Bonus);
			}
			}
			
			//Бонус скорости
			{
			//Если сьел скорость то + или - скорость
			if(Is_Snake_Head_Is_It('!', snake))
			{
				switch(whatSpeed)
				{
					case 0:
					{
						Speed -= 25;
						break;
					}
					case 1:
					{
						Speed += 60;
						break;
					}
				}
				levels.Generate_Bonus(ref levels.X_Speed_Bonus, ref levels.Y_Speed_Bonus);
				whatSpeed = (byte)rand.Next(2);
				isSpeed = false;
				Console.SetCursorPosition(40, 33);
				Console.Write("    ");
				Console.SetCursorPosition(40, 33);
				Console.Write(Speed);
				}

			//Если пустая клетка то поставить +/- скорость
			if(Is_Empty_Cell(levels.X_Speed_Bonus, levels.Y_Speed_Bonus)
				&& !isSpeed 
				&& score >= levels.lvl_2_Score_Need
				&& rand.Next(30) < 3)
			{
				GameField[levels.X_Speed_Bonus, levels.Y_Speed_Bonus] = '!';
				isSpeed = true;
			}

			//Перегенерация скорости если не удалось разместить
			else if (!isSpeed)
			{
				levels.Generate_Bonus(ref levels.X_Speed_Bonus, ref levels.Y_Speed_Bonus);
			}
			}
			
			//перемещение змейки (престановка елементов)
			for(int i = 1; i < snake.nTail; i++)
			{
				(prev[0], snake.sSnakeTail[0, i]) = (snake.sSnakeTail[0, i], prev[0]);
				(prev[1], snake.sSnakeTail[1, i]) = (snake.sSnakeTail[1, i], prev[1]);

				/*temp[0] = snake.sSnakeTail[0, i];
				temp[1] = snake.sSnakeTail[1, i];
				snake.sSnakeTail[0, i] = prev[0];
				snake.sSnakeTail[1, i] = prev[1];
				prev[0] = temp[0];
				prev[1] = temp[1];
				*/

			}



			//заполнение игрового поля--------------------------------------------
			for(int i = 0; i < rows; i++)
			{
				for(int j = 0; j < columns; j++)
				{
					//Змейка (голова)
					if(i == snake.xSnakeHead && j == snake.ySnakeHead)
					{
						GameField[i, j] = 'Q';
					}
					else if(GameField[i, j] == '@' && (score % levels.DoubleFruit_Need_Score) != 0)
					{
						GameField[i, j] = ' ';
						isDoubleFruit = false;
					}
					//Если фрукт / стенка / супер-фрукт / скорость
					else if(GameField[i, j] == '$' || GameField[i, j] == '#' || GameField[i, j] == '@' || GameField[i, j] == '!') { }
					//Хвост или клетки поля
					else
					{
						bool print = false;
						for(int k = 0; k < snake.nTail; k++)
						{
							if(snake.sSnakeTail[0, k] == i && snake.sSnakeTail[1, k] == j)
							{
								print = true;
								GameField[i, j] = '*';
							}
						}
						if(!print)
							GameField[i, j] = ' ';                     //клетки поля
					}

					
					
				}
			}
			//---------------------------------------------------------------------

			//Если скорость меньше доступной сделать её максимально возможной
			if(Speed <= 0)
			{
				Speed = 0;
			}

			return gameOver;
		}




		//Проверка соседних ячеек что бы не было стенок препятствия
		bool Can_Be_Placed_Bonus(int x, int y)
		{
			return ((GameField[x + 1, y] != '#'
							&& GameField[x - 1, y] != '#')
					|| (GameField[x, y + 1] != '#'
							&& GameField[x, y - 1] != '#')
					);
		}

		//Проверка на пустую ячейку
		bool Is_Empty_Cell(int x, int y)
		{
			return (GameField[x, y] == ' ');
		}

		//Соответствует ли координаты головы змеи указанному символу
		bool Is_Snake_Head_Is_It(char x, Snake s)
		{
			return (GameField[s.xSnakeHead, s.ySnakeHead] == x);
		}
	}

}
