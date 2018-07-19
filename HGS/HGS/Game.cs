using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace HGS
{
    /*All enums used in the program */
    public enum possibleKeys { none, left, right, up, down };
    public enum possibleState { skiing, notStarted, finishing, road, gameOver };
    public enum possibleObjects { tree, flags, jump, finish};
    public enum possibleCars { ambulance, truck, ycar, rcar, motorcycle }
    public enum possibleDirections { up, down, left, right}

    abstract class Cars
    {
        public int x;
        public int y;
        public  string imagePath;                   //image location in Debug
        public possibleCars ID;                     //type of car
        public bool alreadyHit = false;             //horace can't hit a car twice
        public int velocity;
        public int mainVelocity;                    //original velocity, velocity can change
        public possibleDirections direction;        //change when overtaking
        public possibleDirections mainDirection;    //right or left
        public bool goToTheMiddle = false;          //flag for overtaking
        public Rectangle rectangle;                 //for collision detection
        
        /*consts for positioning and moving */
        public int upperMiddle = 120;
        public int lowerMiddle = 280;
        public int littleConst = 5;

        /*movement based on car's direction */
        public void move()
        {
            switch (direction)
            {
                case possibleDirections.up:
                    y -= velocity;
                    rectangle.Y -= velocity;

                    if (mainDirection == possibleDirections.right)
                    {
                        x += littleConst;
                        rectangle.X += littleConst;

                        if (y <= upperMiddle)//go up until you reach the middle 
                        {
                            y = upperMiddle;
                            rectangle.Y = upperMiddle;
                            goToTheMiddle = false;
                            direction = mainDirection;
                        }
                    }
                    else
                    {
                        x += littleConst;
                        rectangle.X += littleConst;

                        if (y <= lowerMiddle)
                        {
                            y = lowerMiddle;
                            rectangle.Y = lowerMiddle;
                            goToTheMiddle = false;
                            direction = mainDirection;
                        }

                    }
                    break;
                case possibleDirections.down:
                    y += velocity;
                    rectangle.Y += velocity;

                    if (mainDirection == possibleDirections.right)
                    {
                        x -= littleConst;
                        rectangle.X -= littleConst;
                        if (y >= upperMiddle)
                        {
                            y = upperMiddle;
                            rectangle.Y = upperMiddle;
                            goToTheMiddle = false;
                            direction = mainDirection;
                        }
                    }
                    else
                    {
                        x -= littleConst;
                        rectangle.X -= littleConst;
                        if (y >= lowerMiddle)
                        {
                            y = lowerMiddle;
                            rectangle.Y = lowerMiddle;
                            goToTheMiddle = false;
                            direction = mainDirection;
                        }

                    }
                    break;
                case possibleDirections.left:
                    x -= velocity;
                    rectangle.X -= velocity;
                    break;
                case possibleDirections.right:
                    x += velocity;
                    rectangle.X += velocity;
                    break;
                default:
                    break;
            } 
        }
    }

    /*concrete types of car */
    class Ambulance : Cars
    {
        public Ambulance(int x, int y, possibleDirections dir)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "ambulance.png";
            this.ID = possibleCars.ambulance;
            this.velocity = 26;
            this.mainVelocity = velocity;
            this.direction = dir;
            this.mainDirection = dir;
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width-5, Image.FromFile(imagePath).Height-5);
        }

    }

    class Motorcycle : Cars
    {
        public Motorcycle (int x, int y, possibleDirections dir)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "motorcycle.png";
            this.ID = possibleCars.motorcycle;
            this.velocity = 23;
            this.mainVelocity = velocity;
            this.direction = dir;
            this.mainDirection = dir;
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width - 5, Image.FromFile(imagePath).Height - 5);
        }

    }

    class Truck : Cars
    {
        public Truck(int x, int y, possibleDirections dir)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "truck.png";
            this.ID = possibleCars.truck;
            this.velocity = 15;
            this.mainVelocity = velocity;
            this.direction = dir;
            this.mainDirection = dir;
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width-10, Image.FromFile(imagePath).Height-20);
        }
    }

    class YCar : Cars
    {
        public YCar(int x, int y, possibleDirections dir)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "ycar.png";
            this.ID = possibleCars.ycar;
            this.velocity = 18;
            this.mainVelocity = velocity;
            this.direction = dir;
            this.mainDirection = dir;
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width - 10, Image.FromFile(imagePath).Height - 10);
        }
    }

    class RCar : Cars
    {
        public RCar(int x, int y, possibleDirections dir)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "rcar.png";
            this.ID = possibleCars.rcar;
            this.velocity = 18;
            this.mainVelocity = velocity;
            this.direction = dir;
            this.mainDirection = dir;
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width - 10, Image.FromFile(imagePath).Height - 10);
        }
    }

    /*objects on the slope */
    abstract class MovingObject
    {
        public int x;
        public int y;
        public string imagePath;
        public possibleObjects ID;
        public bool alreadyHit = false; 
        public bool alreadyPenalized = false; //flag for miss-flag penalization

        /*movement */
        public void moveUp()
        {
            y -= 14;
        }

        public void moveUpJump() //when horace is in the air
        {
            y -= 14;
        }
        

    }

    /*concrete objects on the slope */
    class Tree : MovingObject
    {
        public Tree(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "tree.png";
            this.ID = possibleObjects.tree;
        }
    }

    class Jump : MovingObject
    {
        public Jump(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "jump.png";
            this.ID = possibleObjects.jump;
        }
    }

    class Flags : MovingObject
    {
        public Flags(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "flags.png";
            this.ID = possibleObjects.flags;
        }
    }

    class Finish : MovingObject
    {
        public Finish(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "finish.png";
            this.ID = possibleObjects.finish;
        }
    }

    /*Horace on the road */
    class RoadHorace
    {
        public int x;
        public int y;
        public string imagePath;
        public Rectangle rectangle;
        public bool haveSkis = false;
        public bool moveToSlope = false;

        public RoadHorace(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "roadHoraceD1.png";
            this.rectangle = new Rectangle(x, y, Image.FromFile(imagePath).Width - 10, Image.FromFile(imagePath).Height - 10);
        }

    }

    class Horace
    {
        public int x;
        public int y;
        public string imagePath;

        public Horace (int x, int y)
        {
            this.x = x;
            this.y = y;
            this.imagePath = "down.png";
        }

    }

    /*main game class */
    class Scene
    {
        public Scene(int w, int h)
        {
            this.widthW = w;
            this.heightW = h;
            this.yPosition = h;
        }

        public int widthW;
        public int heightW;
        public int previousCarY;        //int flag for changing car's start-positions
        int jumpCount = 0;              //timer for the jump duration
        int yPosition;                  //y position for created objects on the slope
        int xMove = 0;                  //constants for moving horace and objects
        int yMove = 0;
        public int life = 3;            //stats
        public int cash = 30;
        public int score = 100;
        public int hi;
        public int[] allCarPositions = { 60, 120, 170, 230, 280, 330 };     //positions on the road
        public int[] carStartingPositions = { 60, 170, 230, 330 };          //110 and 270 are middle for takovering cars

        public bool moveFlag = true;            //flag for horace's down-movement on the slope
        public bool finishFlag = false;         //flag for the finish appereance 
        public bool jumpFlag = false;           //flag for slowing down during the jump
        public bool gameOver = false;           //flag for ending because of lack of cash or lifes
        public bool roadHoraceHit = false;      //flag for stopping calling the ambulance
        public bool menuAlreadyPainted = false;

        string sString;         //auxiliar string for changing road horace image

        public Random r = new Random();
        public Color color;     //bg-color of the slope

        public Finish finish;
        public Ambulance ambulanceForHorace;
        public Horace horace = new Horace(255,13);
        public RoadHorace roadHorace = new RoadHorace(255, 18);

        public possibleKeys horaceDirection = possibleKeys.down;
        public possibleState state = possibleState.notStarted;

        /*lists of moving objects */
        public List<MovingObject> movingObjects = new List<MovingObject>();
        public List<Cars> cars = new List<Cars>();



        /*METHODS*/

        /*--------------*/
        /*---AUXILIAR---*/
        /*--------------*/

        /*randomize position for new object's on the slope*/
        public int newObjectsPostion()
        {
            int position = r.Next(0, 40)*10;
            return position;
        }

        /*randomize car's starting positions on the road*/
        public int chooseCarStartingPosition()
        {
            int temp = r.Next(0, 4);
            return carStartingPositions[temp]; //above
        }

        /*--------------*/
        /*---MOVEMENT---*/
        /*--------------*/

        /*moving with horace on the road based on the pressed key
        changing horace's pictures*/
        public void moveRoadHorace(possibleKeys pk, int windowWidth, int windowHeight)
        {
            xMove = 10;
            yMove = 4;
            if (roadHorace.haveSkis) sString = "s";
            else sString = "";
            
            switch (pk)
            {
                case possibleKeys.left:
                    if (roadHorace.x > 10)
                    {
                        roadHorace.x -= xMove;
                        roadHorace.rectangle.X -= xMove;
                    }
                    horaceDirection = possibleKeys.down;
                    if (roadHorace.imagePath == string.Format("{0}roadHoraceL1.png", sString)) roadHorace.imagePath = string.Format("{0}roadHoraceL2.png", sString); //making steps
                    else roadHorace.imagePath = string.Format("{0}roadHoraceL1.png", sString);
                    break;
                case possibleKeys.right:
                    if (roadHorace.x < windowWidth - 50)
                    {
                        roadHorace.x += xMove;
                        roadHorace.rectangle.X += xMove;
                    }
                        horaceDirection = possibleKeys.down;
                    if (roadHorace.imagePath == string.Format("{0}roadHoraceR1.png", sString)) roadHorace.imagePath = string.Format("{0}roadHoraceR2.png", sString);
                    else roadHorace.imagePath = string.Format("{0}roadHoraceR1.png", sString);
                    break;
                case possibleKeys.up:
                    horaceDirection = possibleKeys.up;
                    if (roadHorace.y > 18)
                    {
                        roadHorace.y -= xMove;
                        roadHorace.rectangle.Y -= xMove;
                    }
                    if (roadHorace.imagePath == string.Format("{0}roadHoraceU1.png", sString)) roadHorace.imagePath = string.Format("{0}roadHoraceU2.png", sString);
                    else roadHorace.imagePath = string.Format("{0}roadHoraceU1.png", sString);
                    break;
                case possibleKeys.down:
                    if (roadHorace.y < windowHeight-40)
                    {
                        roadHorace.y += xMove;          //unfortunate naming, but it fits the same value
                        roadHorace.rectangle.Y += xMove;
                    }
                        if (roadHorace.imagePath == string.Format("{0}roadHoraceD1.png", sString)) roadHorace.imagePath = string.Format("{0}roadHoraceD2.png", sString);
                    else roadHorace.imagePath = string.Format("{0}roadHoraceD1.png", sString);
                    break;
                default:
                    break;
            }

        }

        /*moving horace on the slope based on the pressed key
        changing horace's picture*/
        public void moveHorace(possibleKeys pk)
        {
            if (horace.y > 190) moveFlag = false;
            else moveFlag = true;

            if (jumpFlag)         //turning off the jumpflag after 10 ticks
            {
                xMove = 8;
                yMove = 2;
                jumpCount++;
                if (jumpCount >= 10)
                {
                    jumpCount = 0;
                    jumpFlag = false;
                }
            }
            else
            {
                xMove = 10;
                yMove = 8;
            }

            switch (pk)
            {
                case possibleKeys.left:
                    if (horace.x > 10) horace.x -= xMove;
                    if (horace.y > 10) horace.y -= yMove;
                    horaceDirection = possibleKeys.down;
                    if (jumpFlag) horace.imagePath = "jumpH.png";
                    else horace.imagePath = "left.png";
                    break;
                case possibleKeys.right:
                    if (horace.x < widthW-75 ) horace.x += xMove;
                    if (horace.y > 10) horace.y -= yMove;
                    horaceDirection = possibleKeys.down;
                    if (jumpFlag) horace.imagePath = "jumpH.png";
                    else horace.imagePath = "right.png";
                    break;
                case possibleKeys.up:
                    horaceDirection = possibleKeys.down;
                    if (horace.y > 10) horace.y -= yMove;
                    if (jumpFlag) horace.imagePath = "jumpH.png";
                    else horace.imagePath = "down.png";
                    break;
                case possibleKeys.down:
                    if (moveFlag) horace.y += xMove;               //unfortunate naming, but needs the same value
                    if (jumpFlag) horace.imagePath = "jumpH.png";
                    else horace.imagePath = "down.png";
                    break;
                default:
                    if (moveFlag) horace.y += xMove;
                    horaceDirection = possibleKeys.down;
                    if (jumpFlag) horace.imagePath = "jumpH.png";
                    else horace.imagePath = "down.png";
                    break;
            }
        }

        /*moving with the ambulance after horace's hit*/
        public void moveWithAmbulanceForHorace(int windowWidth, int windwHeight)
        {
            if (ambulanceForHorace == null)
            {
                ambulanceForHorace = new Ambulance(0, windwHeight - 40, possibleDirections.right);
                cars.Add(ambulanceForHorace);
            }

            ambulanceForHorace.move();

            if (ambulanceForHorace.x > windowWidth - 20)
            {
                roadHoraceHit = false;
                ambulanceForHorace = null;
                roadHorace.x = 255;
                roadHorace.rectangle.X = 255;
                roadHorace.y = 18;
                roadHorace.rectangle.Y = 18;
            }
        }

        /*move all cars on the road*/
        public void moveWithCars(int windowWidth, int windowHeight)
        {
            cars.RemoveAll(item => (item.x < -80) || (item.x > windowWidth));       //delete all unvisible cars

            foreach (Cars c in cars)
            {
                c.move();
            }
        }

        /*move all objects on the slope up*/
        public void moveWithObjects()
        {
            movingObjects.RemoveAll(item => item.y < 0);

            foreach (MovingObject mo in movingObjects)
            {
                if (jumpFlag) mo.moveUpJump();
                else mo.moveUp();
            }
        }

        /*----------------*/
        /*---COLLISIONS---*/
        /*----------------*/

        /*checking horace's distance from different objects on the slope
        used in detectHoraceCollisions*/
        public bool doesHoraceHit(MovingObject mo)
        {
            switch (mo.ID)
            {
                case possibleObjects.flags:
                    if ((Math.Abs(horace.y - mo.y) <= 20) && !mo.alreadyHit)        //constants for fitting
                    {
                        if ((horace.x - mo.x >= 10) && ((horace.x - mo.x <= 120)))
                        {
                            mo.alreadyHit = true;
                            return true;
                        }
                    }

                    break;
                case possibleObjects.finish:
                    if (((horace.y - mo.y) >= 60) && ((horace.y - mo.y) <= 80) && (!mo.alreadyHit)) //constants for fitting
                    {
                        if ((horace.x - mo.x >= 5) && ((horace.x - mo.x <= 120)))
                        {
                            mo.alreadyHit = true;
                            return true;
                        }
                    }
                    break;
                case possibleObjects.jump:
                    if ((Math.Abs(horace.y - mo.y) <= 20) && !mo.alreadyHit) //constants for fitting
                    {
                        if ((((horace.x - mo.x) <= 45) && ((horace.x - mo.x) > 0)) || (((horace.x - mo.x) >= -30) && ((horace.x - mo.x) < 0)))
                        {
                            mo.alreadyHit = true;
                            return true;
                        }
                    }

                    break;
                default: //tree 
                    if ((Math.Abs(horace.y - mo.y) <= 20) && !mo.alreadyHit) //constants for fitting
                    {
                        if (Math.Abs(horace.x - mo.x) <= 33)
                        {
                            mo.alreadyHit = true;
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        /*detecting horace's collisions & signalizing it */
        public void detectHoraceCollisions()
        {
            foreach (MovingObject mo in movingObjects)
            {
                switch (mo.ID)
                {
                    case possibleObjects.tree:
                        if (doesHoraceHit(mo))
                        {
                            life -= 1;
                            SoundPlayer simpleSound = new SoundPlayer("treeCrash.wav");
                            simpleSound.Play();
                        }
                        break;
                    case possibleObjects.flags:
                        if (doesHoraceHit(mo))
                        {
                            score += 10;
                            SoundPlayer simpleSound = new SoundPlayer("flagPass.wav");
                            simpleSound.Play();
                        }

                        break;
                    case possibleObjects.jump:
                        if (doesHoraceHit(mo))
                        {
                            jumpFlag = true;
                            SoundPlayer simpleSound = new SoundPlayer("jump.wav");
                            simpleSound.Play();
                        }
                        break;
                    case possibleObjects.finish:
                        if (doesHoraceHit(mo))
                        {
                            score += 100;
                            SoundPlayer simpleSound = new SoundPlayer("flagPass.wav");
                            simpleSound.Play();
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        /*check two cars intersection*/
        public bool doesCarsIntersect(Cars c1, Cars c2)
        {
            return c1.rectangle.IntersectsWith(c2.rectangle);
        }

        /*detecting road horace collision with cars*/
        public void detectRoadHoraceCollisions(int windowWidth, int windowHeight)
        {
            foreach (Cars car in cars)
            {
                Rectangle overlap = Rectangle.Intersect(roadHorace.rectangle, car.rectangle);
                if (overlap.Width > 17 && car.alreadyHit == false && overlap.Height > 14)
                {
                    car.alreadyHit = true;
                    roadHorace.haveSkis = false;

                    cash -= 10;

                    SoundPlayer simpleSound = new SoundPlayer("carCrash.wav");
                    simpleSound.Play();
                    roadHoraceHit = true;
                }
            }

            if ((roadHorace.y >= windowHeight - 45) && (roadHorace.x >= 200) && (roadHorace.x <= 260) && (roadHorace.haveSkis == false)) //got skis
            {
                roadHorace.haveSkis = true;
                SoundPlayer simpleSound = new SoundPlayer("skiPickUp.wav");
                simpleSound.Play();

            }
            if ((roadHorace.haveSkis) && (roadHorace.y < 30)) //got back with skis
            {
                roadHorace.moveToSlope = true;
            }
        }

        /*checking car collisions and plausible overtaking*/
        public void detectCarCollisions()
        {
            foreach (Cars car1 in cars)
            {
                foreach (Cars car2 in cars)
                {
                    if (doesCarsIntersect(car1, car2) && ((car1.y == allCarPositions[1] && car2.y == allCarPositions[1]) ||
                        (car1.y == allCarPositions[4] && car2.y == allCarPositions[4]))) //crash in the middle
                    {
                        if (car1.velocity < car2.velocity) car2.velocity = car1.velocity; //car2 is faster
                        else car1.velocity = car2.velocity;

                    }
                    else if (doesCarsIntersect(car1, car2) && car1 != car2 && car1.goToTheMiddle == false && car2.goToTheMiddle == false) //crash on edges
                    {
                        if (car1.velocity < car2.velocity) //car2 is faster
                        {
                            car2.goToTheMiddle = true;
                            if (((car2.y >= allCarPositions[0]) && (car2.y <= allCarPositions[1])) || ((car2.y >= allCarPositions[3]) && (car2.y <= allCarPositions[4]))) //faster goes down
                            {
                                car2.direction = possibleDirections.down;
                            }
                            else if (((car2.y <= allCarPositions[2]) && (car2.y >= allCarPositions[1])) || ((car2.y >= allCarPositions[4]) && (car2.y <= allCarPositions[5])))
                            {
                                car2.direction = possibleDirections.up;
                            }
                        }
                        else //car1 is faster
                        {
                            car1.goToTheMiddle = true;
                            if (((car1.y >= allCarPositions[0]) && (car1.y <= allCarPositions[1])) || ((car1.y >= allCarPositions[3]) && (car1.y <= allCarPositions[4]))) //faster goes down
                            {
                                car1.direction = possibleDirections.down;
                            }
                            else if (((car1.y <= allCarPositions[2]) && (car1.y >= allCarPositions[1])) || ((car1.y >= allCarPositions[4]) && (car1.y <= allCarPositions[5])))
                            {
                                car1.direction = possibleDirections.up;
                            }
                        }

                    }
                }
            }
        }

        /*check whether Horace has missed any flag and eventually penalize*/
        public void checkMissedFlags()
        {
            foreach (MovingObject mo in movingObjects)
            {
                if (mo.ID == possibleObjects.flags)
                {
                    if (mo.y < horace.y)
                    {
                        if (!mo.alreadyHit && !mo.alreadyPenalized) score = Math.Max(score - 20, 0);
                        mo.alreadyPenalized = true;
                    }
                }
            }
        }

        /*--------------------*/
        /*---CREATE OBJECTS---*/
        /*--------------------*/

        /*create new car on each spawn click*/
        public void createCars(int windowWidth, int windowHeight)
        {
            int temp = r.Next(0, 6);
            possibleCars pc;

            int xPosition; int yPosition;
            possibleDirections dir;

            int tempY = chooseCarStartingPosition();

            while (tempY == previousCarY) //check for repetitive positions
            {
                tempY = chooseCarStartingPosition();
            }

            previousCarY = tempY;
            yPosition = tempY; //220, 270, 320 ;; 160, 110, 60

            if (yPosition > allCarPositions[2]) //cars movement direction
            {
                xPosition = windowWidth;
                dir = possibleDirections.left;
            }
            else
            {
                xPosition = -60;
                dir = possibleDirections.right;
            }
                
            pc = (possibleCars)temp; //type of car

            switch (pc)
            {
                case possibleCars.ambulance:
                    Ambulance ambulance = new Ambulance(xPosition, yPosition, dir);
                    cars.Add(ambulance);
                    break;
                case possibleCars.truck:
                    Truck truck = new Truck(xPosition, yPosition, dir);
                    cars.Add(truck);
                    break;
                case possibleCars.ycar:
                    YCar ycar = new YCar(xPosition, yPosition, dir);
                    cars.Add(ycar);
                    break;
                case possibleCars.rcar:
                    RCar rcar = new RCar(xPosition, yPosition, dir);
                    cars.Add(rcar);
                    break;
                case possibleCars.motorcycle:
                    Motorcycle motorcycle = new Motorcycle(xPosition, yPosition, dir);
                    cars.Add(motorcycle);
                    break;
                default:
                    break;
            }
        }

        /*create objects on the slope*/
        public void createObject()
        {
            int temp = r.Next(0, 3);
            possibleObjects po;


            if (r.Next(0, 10) < 5)
            {
                int xPosition = newObjectsPostion();

                po = (possibleObjects)temp;
                
                switch (po)
                {
                    case possibleObjects.tree:
                        Tree tree = new Tree(xPosition, yPosition); //yPostition by default
                        movingObjects.Add(tree);
                        break;
                    case possibleObjects.flags:
                        int numberOfFlags = r.Next(0, 6);
                        

                        for (int i = 1; i < numberOfFlags; i++) //deciding how many flags in a row
                        {
                            temp = r.Next(0, 4);
                            int positionFlag = r.Next(0, 11);
                            if (temp < 1)
                            {
                                if (xPosition - 20 > 0) xPosition -= 20;
                            }
                            else
                            {
                                if (xPosition + 20 < widthW) xPosition += 20;                                
                            }
                            yPosition += 50;

                            if ((xPosition > 0) && (xPosition < 350))
                            {
                                Flags flag = new Flags(xPosition, yPosition);
                                movingObjects.Add(flag);
                            }
                        }
                        break;
                    case possibleObjects.jump:
                        Jump jump = new Jump(xPosition, yPosition);
                        movingObjects.Add(jump);
                        break;
                    default:
                        break;
                }
            }
        }

        /*create the finish line*/
        public void createFinish()
        {
            finish = new Finish(250, yPosition);
            movingObjects.Add(finish);
        }

        /*----------*/
        /*DRAW SCENE*/
        /*----------*/

        /*draw everything on the bitmap*/
        public void drawScreen(Graphics g, Font font, Bitmap picture, int windowWidth, int windowHeight)
        {
            if (state == possibleState.road) //road
            {
                Graphics roadPicture = Graphics.FromImage(picture);

                roadPicture.DrawImage(Bitmap.FromFile("road.png"), 0, -70); //background
                if (!roadHoraceHit) roadPicture.DrawImage(Bitmap.FromFile("skis.png"), 230, 385); //skis
                roadPicture.DrawImage(Bitmap.FromFile(roadHorace.imagePath), roadHorace.rectangle); //horace

                foreach (Cars c in cars) //cars
                {
                    string path;
                    if (c.mainDirection == possibleDirections.right) path = string.Format("R{0}", c.imagePath);
                    else path = c.imagePath;

                    roadPicture.DrawImage(Bitmap.FromFile(path), c.rectangle);
                }

                g.DrawImage(picture, 0, 0);
            }

            else if (state == possibleState.skiing) //slope
            {
                color = Color.FromArgb(196, 198, 196);

                Graphics slopePicture = Graphics.FromImage(picture); 

                slopePicture.Clear(color);
                slopePicture.DrawImage(Bitmap.FromFile(horace.imagePath), horace.x, horace.y);

                foreach (MovingObject mo in movingObjects)
                {
                    slopePicture.DrawImage(Bitmap.FromFile(mo.imagePath), mo.x, mo.y);
                }
            }

            g.DrawImage(picture, 0, 0);
        }
    }
}
