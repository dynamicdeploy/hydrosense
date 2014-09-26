# Hydro$ense
September 25, 2014

Hydro$ense is a Hydro-Economic Net Benefit Maximizer. Provided supply and demand for water with the associated economic supply-cost and demand-price, this program solves for the optimal economic water delivery.


## Using the software

### Input
An excel file, either *.xls* or *.xlsx*, containing the following worksheets and data:

* **Supply Curves** - For each supply node two rows of data points representing the marginal supply curves or supply-cost relationship.
* **Demand Curves** - For each demand node two rows of data points representing the marginal demand curves or demand-price relationship.
* **Transportation Costs** - For the number of nodes (supply nodes * demand nodes) two rows of data points representing the transportation-cost from each supply node to each demand node.
* **Transportation Losses** - Same as the transportation costs but defining the transporation-loss from each supply node to each demand node.
* **Initial Guess** - A matrix guess of the supply delivered to each demand node. Rows representing demand nodes and columns representing supply nodes. If a supply node cannot deliver water to a demand node the guess should be zero.

An example problem (ExampleProblem.xlsx) is provided in the software installation directory.


### Model parameters
The GUI provides access to the following solver parameters:

*  **Max Solution Iterations** - The maximum number of iterations the solver will perform.
*  **Convergence Tolerance** - Precision of solver iteration convergence.

#### *Advanced parameters*
* **Numerical Derivative Increment** - ??


### Output
An excel file, either *.xls* or *.xlsx*, the following worksheets will be created or **overwritten** in the output excel file:

* **Optimal Supply** - Matrix of optimal supply quantity from each supply node to each demand node. Rows represent demand nodes and columns represent supply nodes.
* **Optimal Delivery** - Matrix of optimal delivery quantity accounting for transportation losses from each supply node to each demand node.
* **Maximum Net Benefit** - Total benefits of water use minus the total cost of water supply and delivery.


## TO-DO
* Setup code tests, using a testing framework like [NUnit](http://www.nunit.org/)
* Test solver against known example problems
