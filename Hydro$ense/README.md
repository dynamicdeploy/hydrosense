# Hydro$ense
September 30, 2014

Hydro$ense is a Hydro-Economic Net Benefit Maximizer. Provided supply and demand for water with the associated economic supply-cost and demand-price, this program solves for the economic partial-equilibrium solution by maximizing the Consumer and Producer Surplus.

The Partial-Equilibrium solution is determined by maximizing the Consumer and Producer Surplus, referred to as the Objective Function, subject to the physical and water management constraints for the Hydro-Economic problem being analyzed. The Objective Function can also be defined as the sum of all the benefits accrued through water use by all of the demanders, minus the costs of providing water from the suppliers and the costs of transporting the water from the suppliers to demanders. The physical constraints include limits on the water available as a supply, and the relationship that defines the loss of water as it is transported from water supplies to water demands, often referred to as the transportation losses. The water management constraints include the limits on the amount of water that demanders are allowed to use as defined by the water rights administration for the problem being analyzed. The optimal (maximum) solution is then determined as the point where the derivative of the Objective Function with respect to the amounts of water provided between the suppliers and demanders (referred to as the decision variables) is equal to zero, as long as this solution is within the physical and management constraints.

The optimal solution must be determined in an iterative fashion utilizing a search algorithm. The search algorithm used in the Hydro$ense solver employs a Gradient Descent method that utilizes numerical approximations of the first and second derivatives of the Objective Function with respect to the decision variables. The solution proceeds by developing an initial guess for the optimal decision variables which is used to estimate the first and second derivatives of the Objective Function with respect to the array of decision variables. The decision variables are then updated by solving the linear system of equations as:

>    **{dv^i} = {dv^(i-1)} - [(Δ^2 OF)/Δdv^2]^(-1) x {ΔOF/Δdv}**

Where:

**dv^i** = the updated array of the estimated optimal decision variables for iteration, i, of the solution;

**{ΔOF/Δdv}** = the numerical estimates of the first derivative of the Objective Function (OF) with respect to the estimate of the optimal decision variables at iteration, i-1; and

**[(Δ^2 OF)/Δdv^2 ]^(-1)** = the inverse of the matrix containing the numerical estimates of the second derivatives of the Objective Function (OF) with respect to the estimate of the optimal decision variables at iteration, i-1.

At the end of each iteration, the updated optimal solution is checked to make sure that all of the problem constraints are met. If an updated decision variable is outside its constraint, the decision variable is set to its constraint limit, which is then used as the optimal set of decision variables for the next iteration in the solution.

To aid in converging towards a stable solution, an adjustment to the diagonal values of the matrix representing the second derivatives of the Objective Function with respect to the decision variables is performed utilizing a Marquardt adjustment, defined as:

>    **e^((i-500) x Δdv)**

Where:

**i** - the number of iterations;

**Δdv** =  the incremental change in the decision variable used to calculate the numerical estimates of the first and second derivatives of the Objective Function;

The optimal solver will iterate towards an optimal solution using the procedure described above until the change in the values of the Objective Function and decision variables meet a user defined convergence tolerance, or the user defined maximum number of iterations is reached. 


## Using the software

### Input
An excel file, either *.xls* or *.xlsx*, containing the following worksheets and data:

* **Supply Curves** - For each supply node two rows of data points representing the marginal cost function. Entered as arrays of flows and marginal costs for each supply node.
* **Demand Curves** - For each demand node two rows of data points representing the marginal price function. Entered as arrays of flows and marginal prices for each demand node.
* **Transportation Losses** - The transportation losses associated with moving water from a supply node to a demand node. Entered as an array of flows from each supply node and an array of flows arriving at each demand node.
* **Transportation Costs** - The transportation costs associated with moving water from a supply node to a demand node. Entered as an array of flows from each supply node and an array of marginal costs to each demand node.
* **Initial Guess** - A matrix guess of the optimal supply to each demand node. Rows representing demand nodes and columns representing supply nodes. If a supply node cannot deliver water to a demand node the guess should be zero.

An example problem (ExampleProblem.xlsx) is provided in the software installation directory.


### Model parameters
The GUI provides access to the following solver parameters:

*  **Max Solution Iterations** - The maximum number of iterations the solver will perform. The default is 5000.
*  **Convergence Tolerance** - Precision of solver iteration convergence. The default is 0.015.

#### *Advanced parameters*
* **Numerical Derivative Increment** - the incremental change in the decision variable used to calculate the numerical estimates of the first and second derivatives of the Objective Function. The default is 0.01.


### Output
An excel file, either *.xls* or *.xlsx*, the following worksheets will be created or **overwritten** in the output excel file:

* **Optimal Supply** - Matrix of optimal supply to each demand node. Rows represent demand nodes and columns represent supply nodes.
* **Optimal Delivery** - Matrix of optimal delivery accounting for transportation losses from the supply node to the demand node.
* **Maximum Net Benefit** - The Objective Function value or the sum of the total benefits accrued through water use minus the total costs of water supply and transportation losses.


## TO-DO
* Setup code tests, using a testing framework like [NUnit](http://www.nunit.org/)
* Test solver against additional problems with known solutions
* Improve solution convergence for a wider range of initial guesses


## Acknowledgments
This project is based on methodologies developed by the Bureau of Reclamation and University of Idaho - Idaho Water Resources Research Institute, and funded by the Reclamation Research and Development Group.
