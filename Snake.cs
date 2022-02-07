using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
	class Snake
	{
		public int[,] sSnakeTail = new int[2, 99];	//Координаты хвоста змеи
		public int xSnakeHead, ySnakeHead;			//Координаты головы змеи
		public int nTail;							//Количество елементов хвоста змеи

		//Инициализация
		public Snake(){
			//Выставление змеи по центру
			xSnakeHead = World.rows / 2 + 1;		
			ySnakeHead = World.columns / 2;
		}
	}
}
