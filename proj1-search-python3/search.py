# search.py
# ---------
# Licensing Information:  You are free to use or extend these projects for
# educational purposes provided that (1) you do not distribute or publish
# solutions, (2) you retain this notice, and (3) you provide clear
# attribution to UC Berkeley, including a link to http://ai.berkeley.edu.
# 
# Attribution Information: The Pacman AI projects were developed at UC Berkeley.
# The core projects and autograders were primarily created by John DeNero
# (denero@cs.berkeley.edu) and Dan Klein (klein@cs.berkeley.edu).
# Student side autograding was added by Brad Miller, Nick Hay, and
# Pieter Abbeel (pabbeel@cs.berkeley.edu).


"""
In search.py, you will implement generic search algorithms which are called by
Pacman agents (in searchAgents.py).
"""

import util

class SearchProblem:
    """
    This class outlines the structure of a search problem, but doesn't implement
    any of the methods (in object-oriented terminology: an abstract class).

    You do not need to change anything in this class, ever.
    """

    def getStartState(self):
        """
        Returns the start state for the search problem.
        """
        util.raiseNotDefined()

    def isGoalState(self, state):
        """
          state: Search state

        Returns True if and only if the state is a valid goal state.
        """
        util.raiseNotDefined()

    def getSuccessors(self, state):
        """
          state: Search state

        For a given state, this should return a list of triples, (successor,
        action, stepCost), where 'successor' is a successor to the current
        state, 'action' is the action required to get there, and 'stepCost' is
        the incremental cost of expanding to that successor.
        """
        util.raiseNotDefined()

    def getCostOfActions(self, actions):
        """
         actions: A list of actions to take

        This method returns the total cost of a particular sequence of actions.
        The sequence must be composed of legal moves.
        """
        util.raiseNotDefined()


def tinyMazeSearch(problem):
    """
    Returns a sequence of moves that solves tinyMaze.  For any other maze, the
    sequence of moves will be incorrect, so only use this for tinyMaze.
    """
    from game import Directions
    s = Directions.SOUTH
    w = Directions.WEST
    print("Start:", problem.getStartState())
    print("Is the start a goal?", problem.isGoalState(problem.getStartState()))
    print("Start's successors:", problem.getSuccessors(problem.getStartState()))
    print("Cost of Actions:", problem.getCostOfActions([s, s, w, s, w, w, s, w]))
    return  [s, s, w, s, w, w, s, w]

def depthFirstSearch(problem):
    """
    Search the deepest nodes in the search tree first.

    Your search algorithm needs to return a list of actions that reaches the
    goal. Make sure to implement a graph search algorithm.

    To get started, you might want to try some of these simple commands to
    understand the search problem that is being passed in:

    print("Start:", problem.getStartState())
    print("Is the start a goal?", problem.isGoalState(problem.getStartState()))
    print("Start's successors:", problem.getSuccessors(problem.getStartState()))
    """
    "*** YOUR CODE HERE ***"
    path = []
    myStackForDFS = util.Stack()
    myClosedSetForDFS = set()

    myStackForDFS.push((problem.getStartState(), path))
    while not myStackForDFS.isEmpty():
        if myStackForDFS.isEmpty():
            print("psyche u failed")
            return None

        myCurrentState = myStackForDFS.pop()
        if problem.isGoalState(myCurrentState[0]):
            print("lol")
            print(myCurrentState[1])
            return myCurrentState[1]

        if myCurrentState[0] not in myClosedSetForDFS:
            myClosedSetForDFS.add(myCurrentState[0])
            myNeighbors = problem.getSuccessors(myCurrentState[0])
            for i in myNeighbors:
                newPath = myCurrentState[1] + [i[1]]
                myStackForDFS.push([i[0], newPath])
    return None

def breadthFirstSearch(problem):
    """Search the shallowest nodes in the search tree first."""
    "*** YOUR CODE HERE ***"
    path = []
    myStackForBFS = util.Queue()
    myClosedSetForBFS = set()

    myStackForBFS.push((problem.getStartState(), path))
    while not myStackForBFS.isEmpty():
        if myStackForBFS.isEmpty():
            print("psyche u failed")
            return None

        myCurrentState = myStackForBFS.pop()
        if problem.isGoalState(myCurrentState[0]):
            return myCurrentState[1]

        if myCurrentState[0] not in myClosedSetForBFS:
            myClosedSetForBFS.add(myCurrentState[0])
            myNeighbors = problem.getSuccessors(myCurrentState[0])
            for i in myNeighbors:
                newPath = myCurrentState[1] + [i[1]]
                myStackForBFS.push([i[0], newPath])
    return None

def uniformCostSearch(problem):
    """Search the node of least total cost first."""
    "*** YOUR CODE HERE ***"
    path = []
    myStackForUCS = util.PriorityQueue()
    myClosedSetForUCS = set()

    myStackForUCS.push([problem.getStartState(), path], 0)
    while not myStackForUCS.isEmpty():
        if myStackForUCS.isEmpty():
            print("psyche u failed")
            return None

        myCurrentState = myStackForUCS.pop()
        if problem.isGoalState(myCurrentState[0]):
            print("lol")
            print(myCurrentState[1])
            return myCurrentState[1]

        if myCurrentState[0] not in myClosedSetForUCS:
            myClosedSetForUCS.add(myCurrentState[0])
            myNeighbors = problem.getSuccessors(myCurrentState[0])
            for i in myNeighbors:
                newPath = myCurrentState[1] + [i[1]]
                myStackForUCS.push([i[0], newPath], problem.getCostOfActions(newPath))
    return None

def nullHeuristic(state, problem=None):
    """
    A heuristic function estimates the cost from the current state to the nearest
    goal in the provided SearchProblem.  This heuristic is trivial.
    """
    return 0

def aStarSearch(problem, heuristic=nullHeuristic):
    """Search the node that has the lowest combined cost and heuristic first."""
    "*** YOUR CODE HERE ***"
    path = []
    myStackForAStar = util.PriorityQueue()
    myClosedSetForAStar = set()

    myStackForAStar.push([problem.getStartState(), path], heuristic(problem.getStartState(), problem))
    while not myStackForAStar.isEmpty():
        if myStackForAStar.isEmpty():
            print("psyche u failed")
            return None

        myCurrentState = myStackForAStar.pop()
        if problem.isGoalState(myCurrentState[0]):
            print("lol")
            print(myCurrentState[1])
            return myCurrentState[1]

        if myCurrentState[0] not in myClosedSetForAStar:
            myClosedSetForAStar.add(myCurrentState[0])
            myNeighbors = problem.getSuccessors(myCurrentState[0])
            for i in myNeighbors:
                newPath = myCurrentState[1] + [i[1]]
                myStackForAStar.push([i[0], newPath], problem.getCostOfActions(newPath) + heuristic(i[0], problem))
    return None


# Abbreviations
bfs = breadthFirstSearch
dfs = depthFirstSearch
astar = aStarSearch
ucs = uniformCostSearch
