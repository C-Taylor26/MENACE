using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1._0
{
    public partial class Form1 : Form   
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        public void btnPlay_Click(object sender, EventArgs e)
        {
            cbLearning.Hide();
            btnPlay.Hide();
            List list = new List();
            int x = 0;
            int o = 0;
            int d = 0;
            lstResults.Items.Clear();
            //check number of games to be played
            string strNumGames = txtNumGames.Text;
            int numGames;
            //check not null and value
            bool isInt = int.TryParse(strNumGames, out numGames);
            if (numGames > 0)
            {
                for (int j = 0; j < numGames; j++) //Each loop is one comepleted game
                {
                    list = reset(list);

                    int choice;
                    {
                        //txtTotal.Clear();
                        txtResult.Clear();
                        pos1.Clear();
                        pos2.Clear();
                        pos3.Clear();
                        pos4.Clear();
                        pos5.Clear();
                        pos6.Clear();
                        pos7.Clear();
                        pos8.Clear();
                        pos9.Clear();
                    }//Clears form
                    bool nFirst = false;
                    //decide who should go first based of numGames
                    if (j % 2== 0)
                    {
                        nFirst = true;
                    }
                    do //echo loop is a turn for both O's and X's
                    {
                        //gameEnd() checked each turn 
                        if (nFirst == true)
                        {
                            choice = turnN(); //Gets choice
                            takeTurn('O', choice); //Adds choice to the game board
                            if (cbShowUpdates.Checked == true)
                            {
                                Application.DoEvents(); //used to show the updates as each move is played
                            }
                            if (gameEnd() == true)
                                break;

                            choice = turnC(list);
                            list = saveState(list, choice);
                            takeTurn('X', choice);

                            if (cbShowUpdates.Checked == true)
                            {
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            choice = turnC(list);
                            list = saveState(list, choice);
                            takeTurn('X', choice);

                            if (cbShowUpdates.Checked == true)
                            {
                                Application.DoEvents();
                            }
                            if (gameEnd() == true)
                                break;
                            choice = turnN();
                            takeTurn('O', choice);
                            if (cbShowUpdates.Checked == true)
                            {
                                Application.DoEvents();
                            }
                        }
                        
                    }
                    while (gameEnd() == false);

                    txtResult.Text = checkWin(); //lets player know who won the round
                    lstResults.Items.Add(txtResult.Text); //Displays result
                    label2.Text = "Games Played: " + (j + 1); 
                    txtTotal.Text = "X: " + x + ", O: " + o + ", D: " + d;

                    if (txtResult.Text == "X's Win!")
                    {
                        x++; 
                        list = learning(list, 2);
                    }
                    else if (txtResult.Text == "O's Win!")
                    {
                        o++;
                        list = learning(list, -1);
                    }
                    else
                    {
                        d++;
                        list = learning(list, 1);
                    }
                }
                txtTotal.Text = "X: " + x + ", O: " + o + ", D: " + d;
                cbLearning.Show();
                btnPlay.Show();
            }
            else
            {
                MessageBox.Show("Please enter an int > 0");
            }
            
        }

        public int turnN()
        {
            //Naughts are a random player
            int choice;

            Random rand = new Random();
            do //gets random turn
            {
                choice = rand.Next(1, 10);
            }
            while (getChar(choice) != "");//checks that the turn is valid and not already taken

            return choice;
        }

        public int turnC(List list)
        {
            int count = 0;
            int choice =0;
            Random rand = new Random();
            GameState currentState = getState(list);
            int[] choices = currentState.moves;
            int num = choices[0];
            int[] moves = new int[9];

            //finds best move from moves that have worked previously
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i] > num)
                {
                    num = choices[i];
                }
            }
            //adds all the best to moves[]
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i] == num )
                {
                    moves[i] = 1;
                }
            }
            //selects a space that is in moves[] and is not taken
            do
            {
                do
                {
                    choice = rand.Next(1, 10);
                    count++;
                } while (getChar(choice) != "");
                if (count > 500)
                {
                    break;
                }
            } while (moves[choice - 1] != 1);

            return choice;
        }

        public bool gameEnd()
        {
            int count = 0;
            for (int i = 1; i < 10; i++)
            {
                if (getChar(i) != "")
                {
                    count++;
                }
            }
            if (count == 9)//ends game if all spaces are full
            {
                return true;
            }
            else if (checkWin() != "Its a draw")
            {
                return true;
            }

            return false;
        }

        public string checkWin()
        {
            //checks the various ways in which a player could win
            string result = "";
            if (pos1.Text == pos2.Text && pos2.Text == pos3.Text
                || pos1.Text == pos4.Text && pos4.Text == pos7.Text
                || pos1.Text == pos5.Text && pos5.Text == pos9.Text)
            {
                if (pos1.Text != "")
                {
                    result = pos1.Text + "'s Win!"; //return the winning character (X/O)
                    return result;
                }

            }
            else if (pos4.Text == pos5.Text && pos5.Text == pos6.Text
                    || pos2.Text == pos5.Text && pos5.Text == pos8.Text
                    || pos3.Text == pos5.Text && pos5.Text == pos7.Text)
            {
                if (pos5.Text != "")
                {
                    result = pos5.Text + "'s Win!";
                    return result;
                }
            }
            else if (pos7.Text == pos8.Text && pos8.Text == pos9.Text
                    || pos3.Text == pos6.Text && pos6.Text == pos9.Text)
            {
                if (pos9.Text != "")
                {
                    result = pos9.Text + "'s Win!";
                    return result;
                }

            }

            return "Its a draw"; //no winning outcome found
        }

        public void takeTurn(char a, int choice)
        {
            switch (choice) //places charachter in chosen posistion
            {
                case 1:
                    pos1.Text = Convert.ToString(a);
                    break;
                case 2:
                    pos2.Text = Convert.ToString(a);
                    break;
                case 3:
                    pos3.Text = Convert.ToString(a);
                    break;
                case 4:
                    pos4.Text = Convert.ToString(a);
                    break;
                case 5:
                    pos5.Text = Convert.ToString(a);
                    break;
                case 6:
                    pos6.Text = Convert.ToString(a);
                    break;
                case 7:
                    pos7.Text = Convert.ToString(a);
                    break;
                case 8:
                    pos8.Text = Convert.ToString(a);
                    break;
                case 9:
                    pos9.Text = Convert.ToString(a);
                    break;
            }
        }

        public List saveState(List list, int choice)//Finds excisting game state or saves it and returns the state object
        {
            GameState currentState = getState(list);
            currentState.used = true;
            currentState.posPlayed = choice;
            return list;
        }

        public GameState getState(List list)//returns the possible moves that can be made in current state
        {

            GameState currentState = new GameState();
            GameState checkState = list.firstNode;
            int[] state = new int[9];

            for (int i = 0; i < state.Length; i++) //finds the state of the board
            {
                if (getChar(i + 1) == "")
                {
                    state[i] = 0; //No entry in position 
                }
                else
                {
                    if (getChar(i + 1) == "X")
                    {
                        state[i] = 1; //X in posistion
                    }
                    else
                    {
                        state[i] = 2; //O in position
                    }
                }
            }
            currentState.state = state;

            currentState.moves = getMoves();

            //find if state[] already exists within list
            if (list.firstNode == null)//nothing in list
            {
                list.firstNode = currentState;
            }
            else
            {
                //search list for state
                while (checkState != null)
                {
                    if (compare(checkState, state) == true)//when found CurrentState becomes the matching game state
                    {
                        currentState = checkState;
                        return currentState;
                    }

                    checkState = checkState.next;
                }
                //no state found so currentState is added to the list
                insertBeginning(list, currentState);
                return currentState;
            }
            return currentState;
        }

        public string getChar(int choice)
        {
            switch (choice)
            {
                case 1:
                    return pos1.Text;

                case 2:
                    return pos2.Text;

                case 3:
                    return pos3.Text;

                case 4:
                    return pos4.Text;

                case 5:
                    return pos5.Text;

                case 6:
                    return pos6.Text;

                case 7:
                    return pos7.Text;

                case 8:
                    return pos8.Text;

                case 9:
                    return pos9.Text;
            }
            return null;
        }

        public int[] getMoves()//function makes new array of possible moves
        {
            int[] moves = new int[9];
            string text = "";

            for (int i = 0; i < 9; i++)
            {
                switch (i)
                {
                    case 0:
                        text = pos1.Text;
                        break;
                    case 1:
                        text = pos2.Text;
                        break;
                    case 2:
                        text = pos3.Text;
                        break;
                    case 3:
                        text = pos4.Text;
                        break;
                    case 4:
                        text = pos5.Text;
                        break;
                    case 5:
                        text = pos6.Text;
                        break;
                    case 6:
                        text = pos7.Text;
                        break;
                    case 7:
                        text = pos8.Text;
                        break;
                    case 8:
                        text = pos9.Text;
                        break;
                }

                if (text == "")
                {
                    moves[i] = 1;//position is avalible
                }
                else
                {
                    moves[i] = 0;//posistion is taken
                }
            }


            return moves;
        }

        public void insertBeginning(List list, GameState newState)
        {
            if (list.firstNode == null)
            {
                list.firstNode = newState;
            }
            else
            {
                newState.next = list.firstNode;
                list.firstNode = newState;
            }
        }

        public List learning(List list, int outcome)
        {
            if (cbLearning.Checked == true)
            {
                //search for "used == y"
                GameState currentState = list.firstNode;

                while (currentState != null)
                {
                    if (currentState.used == true)
                    {
                        currentState.moves[currentState.posPlayed - 1] = currentState.moves[currentState.posPlayed - 1] + outcome;
                    }
                    currentState = currentState.next;
                }
            }
            return list;
        }

        public List reset(List list)
        {
            GameState state = list.firstNode;

            while (state != null)
            {
                state.used = false;
                state.posPlayed = 0;
                state = state.next;
            }


            return list;
        }

        public bool compare(GameState checkState, int[] state)
        {
            int matches = 0;
            for (int s = 0; s < state.Length; s++)
            {
                if(checkState.state[s] == state[s])
                {
                    matches++;
                }
            }
            
            if (matches == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    }

    public class List
    {
        public GameState firstNode;
    }

    public class GameState
    {
        public bool used;
        public int posPlayed;
        public int[] state;
        public int[] moves;
        public GameState next;
    }
}
