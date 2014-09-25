# Hydro$ense
September 25, 2014

Hydro$ense is ...


## Using the software

### Input
An excel file, either *.xls* or *.xlsx*, containing the following worksheets and data:

* **Supply Curves** - For each supply node two rows of data points representing the marginal supply curves.
* **Demand Curves** - For each demand node two rows of data points representing the marginal demand curves.
* **Transportation Costs** - For the number of nodes (supply nodes * demand nodes) two rows of data points representing the supply cost to transport water volumes from each supply node to each demand node.
* **Transportation Losses** - Same as the transportation costs but defining the supply loss from each supply node to each demand node.
* **Initial Guess** - A matrix guess of the supply delivered to each demand node. Rows representing each demand node and columns representing each supply node. If no flow can occur between a supply node to a demand node the guess should be zero.

An example input spreadsheet is provided in the installation directory.


### Model parameters
The GUI provides access to the following solver parameters:

*  **Max Solution Iterations** - The maximum number of iterations the solver will perform.
*  **Convergence Tolerance** - Precision of solver iteration convergence.

##### *Advanced parameters*
* **Numerical Derivative Increment** - ??


### Output
An excel file, either *.xls* or *.xlsx*, the following worksheets will be created or **overwritten** in the output excel file:

* **Quantity Delivered** - Matrix of water delivered. Rows represent each demand node and columns represent each supply node.
