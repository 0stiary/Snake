using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
	class Levels:World
	{
		Random rand = new Random();
		public int[,] lvl_1_Wall, lvl_2_Wall = new int [2,10];          //Координаты препятсвия 1го,	2го,	3го		уровней
		public int FruitX, FruitY;                                      //Координаты фрукта		Ряд | Столб
		//public int X_GameOver_Bonus, Y_GameOver_Bonus;					//Конец игры - бонус
		public int X_Speed_Bonus, Y_Speed_Bonus;            //скорость - бонус
		public int X_DoubleFruit_Bonus, Y_DoubleFruit_Bonus, DoubleFruit_Need_Score = 30;			//двойной фрукт - бонус
		public int lvl_1_Score_Need = 100, lvl_2_Score_Need = 500;		//Сколько очков нужно что бы перейти на 1й уровень
		int randoms;

		//Иницализация
		public Levels()
		{
			Level_0();
			Level_1();
			Level_2();
		}

		public void Generate_Bonus(ref int x, ref int y){
			x = rand.Next(1, rows-1);
			y = rand.Next(1, columns-1);
		}

		//Иницализация 0го уровня
		void Level_0(){
			Generate_Bonus(ref FruitX, ref FruitY);
		}
		
		//Иницализация 1го уровня
		void Level_1(){

			//Горизонтально или вертикально
			randoms = rand.Next(2);

			switch(randoms)
			{
				//горизонтальное препятствие
				case 0:
				{
					lvl_1_Wall = new int[2, 26];
					int XRandomWallPoint = rand.Next(1, rows - 1);
					int YRandomWallPoint = rand.Next(columns - (lvl_1_Wall.Length / 2));

					for(int i = 0; i < lvl_1_Wall.Length / 2; i++)
					{

						lvl_1_Wall[0, i] = XRandomWallPoint;
						lvl_1_Wall[1, i] = YRandomWallPoint + i;
					}
					break;
				}
				//вертикальнаое препятствие
				case 1:
				{
					lvl_1_Wall = new int[2, 13];
					int XRandomWallPoint = rand.Next(rows - (lvl_1_Wall.Length / 2));
					int YRandomWallPoint = rand.Next(1, columns - 1);

					for(int i = 0; i < lvl_1_Wall.Length / 2; i++)
					{
						lvl_1_Wall[0, i] = XRandomWallPoint + i;
						lvl_1_Wall[1, i] = YRandomWallPoint;
					}
					break;
				}
			}

			Generate_Bonus(ref X_DoubleFruit_Bonus, ref Y_DoubleFruit_Bonus);
		}

		//Иницализация 2го уровня
		void Level_2(){

			//Слева или справа
			randoms = rand.Next(2);

			switch(randoms)
			{
				//слева направо диагональ -  препятствие
				case 0:
				{
					int XRandomWallPoint = rand.Next(rows - (lvl_2_Wall.Length / 2));
					int YRandomWallPoint = rand.Next(columns - (lvl_2_Wall.Length / 2));

					for(int i = 0; i < lvl_2_Wall.Length / 2; i++)
					{
						lvl_2_Wall[0, i] = XRandomWallPoint + i;
						lvl_2_Wall[1, i] = YRandomWallPoint + i;
					}
					break;
				}
				//справа налево препятствие препятствие
				case 1:
				{
					int XRandomWallPoint = rand.Next(rows - (lvl_2_Wall.Length / 2));
					int YRandomWallPoint = rand.Next((lvl_2_Wall.Length / 2), columns);

					for(int i = 0; i < lvl_2_Wall.Length / 2; i++)
					{
						lvl_2_Wall[0, i] = XRandomWallPoint + i;
						lvl_2_Wall[1, i] = YRandomWallPoint - i;
					}
					break;
				}
			}

			Generate_Bonus(ref X_Speed_Bonus, ref Y_Speed_Bonus);
		}

	}
}
