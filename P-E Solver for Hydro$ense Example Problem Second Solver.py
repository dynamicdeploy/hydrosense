#-------------------------------------------------------------------------------
# Name:        Simple Non-Linear Solver
# Purpose:     Determine the optimal allocation of water supplied from
#              a set of Supply Nodes to a set of Demand Nodes by
#              maximizing the Producer and Consumer Surplus for the system
#
# Author:      tracy
#
# Created:     25/07/2013
# Copyright:   (c) tracy 2013
#-------------------------------------------------------------------------------

def main():
    pass

if __name__ == '__main__':
    main()
#-------------------------------------------------------------------------
#
#  Global variables for this module are:
#      deltai = increment used to calculate numerical integration values
#      deltad = increment used to calcuate numerical differential values
#      tolerance = convergence tolerance for arriving at Optimum
#
#  Note: tolerance must be >> deltad to assure that convergence criteria
#        can be met
#
#-------------------------------------------------------------------------
global deltai
global deltad
global tolerance
global MCS
global MCD
global TL
global TC

class SupplyNodes():
#-------------------------------------------------------------------------
#
# This class defines the characteristics of a supply node.  This includes
# initiating the class parameters, as well as the defining the marginal
# cost function and the supply node constraints
#
#-------------------------------------------------------------------------
  def __init__(self, Ns, Np, x, y):
#
#  Initiatilzes the array parameters defining the marginal cost curve
#  and defines the Constraint on the supply node
#
    self.NumSupplys = Ns
    self.NumPoints = list()
    self.Xc = list()
    self.Yc = list()
    self.YIc = list()
    nc = 0
    for i in range (0, Ns):
        self.NumPoints.append(Np[i])
        if i < 1: nc = 0
        else: nc += Np[i-1]
        k = 0
        for j in range (nc, nc + Np[i]):
            self.Xc.append(x[j])
            self.Yc.append(y[j])
            if (k > 0) : Value += ((y[j] + y[j-1])/2.0)*(x[j] - x[j-1])
            else: Value = 0.0
            self.YIc.append(Value)
            k += 1
#
#  End initialization of variables in Class SupplyNodes
#
#
#------------Define Limiting Value-------------------
#
  def Limit(self,n):
    nc = 0
    i = 0
    while i <= n:
        nc += self.NumPoints[i]
        i += 1;
    return self.Xc[nc - 1]
#
#----------Define the function for returning the marginal cost--------------
#
  def MarginalCost(self, n, q):
    Cost = 0.0
    i = 0
    nc = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    k = nc
    while self.Xc[k] < q:
        k += 1
    slope = (self.Yc[k] - self.Yc[k-1])/(self.Xc[k] - self.Xc[k-1])
    Cost = self.Yc[k-1] + (q - self.Xc[k-1])*slope
    return Cost
#
#-------Define the function for returning the integrated supply cost--------
#
  def IntegratedCost(self, n, q):
    Cost = 0.0
    i = 0
    nc = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    k = nc
    while self.Xc[k] < q: k += 1
    slope = (self.YIc[k] - self.YIc[k-1])/(self.Xc[k] - self.Xc[k-1])
    Cost = self.YIc[k-1] + (q - self.Xc[k-1])*slope
    return Cost
#
#  End Definitions for the SupplyNode Class
#

class DemandNodes():
#-------------------------------------------------------------------------
#
# This class defines the characteristics of a supply node.  This includes
# initiating the class parameters, as well as the defining the marginal
# cost function and the supply node constraints
#
#-------------------------------------------------------------------------

  def __init__(self, Nd, Np, x, y):
#
#  Initiatilzes the array parameters defining the marginal cost curve
#  and integrated cost curve
#
    self.NumDemands = Nd
    self.NumPoints = list()
    self.Xc = list()
    self.Yc = list()
    self.YIc = list()
    Value = 0.0
    nc = 0
    for i in range (0, Nd):
        self.NumPoints.append(Np[i])
        if i < 1: nc = 0
        else: nc += Np[i-1]
        k = 0
        for j in range (nc, nc + Np[i]):
            self.Xc.append(x[j])
            self.Yc.append(y[j])
            if (k > 0) : Value += ((y[j] + y[j-1])/2.0)*(x[j] - x[j-1])
            else: Value = 0.0
            self.YIc.append(Value)
            k += 1
#
#  End initialization of variables in Class DemandNodes
#
#-------------------------------------------------------------------------
#
  def QuantValue(self, n):
    QuantV = list()
    nc = 0
    i = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    for j in range (nc, nc+self.NumPoints[n]):
        QuantV.append(self.Xc[j])
    return QuantV
#
#---------Define Limiting Value---------------------
#
  def Limit(self,n):
    nc = 0
    i = 0
    while i <= n:
        nc += self.NumPoints[i]
        i += 1;
    return self.Xc[nc - 1]
#
#-------------------------------------------------------------------------
#
  def MarginalCost(self, n, q):
    Cost = 0
    i = 0
    nc = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    k = nc
    while self.Xc[k] < q: k += 1
    slope = (self.Yc[k] - self.Yc[k-1])/(self.Xc[k] - self.Xc[k-1])
    Cost = self.Yc[k-1] + (q - self.Xc[k-1])*slope
    return Cost
#
#-------Define the function for returning the integrated supply cost--------
#
  def IntegratedCost(self, n, q):
    Cost = 0.0
    i = 0
    nc = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    k = nc
    while self.Xc[k] < q: k += 1
    slope = (self.YIc[k] - self.YIc[k-1])/(self.Xc[k] - self.Xc[k-1])
    Cost = self.YIc[k-1] + (q - self.Xc[k-1])*slope
    return Cost
#
#  End Definitions for the DemandNodes Class
#
class TransportationCosts ():
#
# This class represents the marginal cost of transporting water between
# supply node i to demand node j.
#
  def __init__(self, Np, Ns, Nd, x, y):
    self.NumSupplys = Ns
    self.NumDemands = Nd
    self.NumPoints = list()
    self.Quant = list()
    self.TC = list()
    self.TIc = list()
    Value = 0.0
    nc = 0
    for i in range (0, Nd):
        for j in range (0, Ns):
            self.NumPoints.append(Np[i*Ns+j])
#
#  Need to calculate integrated marginal cost of water transportation
#
            for k in range (0, Np[i*Ns+j]):
                self.Quant.append(x[nc+k])
                self.TC.append(y[nc+k])
                if (k > 0) : Value += ((y[nc+k] + y[nc+k-1])/2.0)*(x[nc+k] - x[nc+k-1])
                else: Value = 0.0
                self.TIc.append(Value)
            nc += Np[i*Ns+j]
#
#  End initialization of the TransportationCost Class
#
#  Define the upper limit for the flow rate between Supply node j and
#  Demand node i
#
  def Limit(self,n):
    nc = 0
    i = 0
    while i <= n:
        nc += self.NumPoints[i]
        i += 1;
    return self.Quant[nc - 1]
#
#  Define the number of points for each transportation link
#
  def TransPoints(self, i):
    return self.NumPoints[i]

  def TransCostRelationship(self, n, k):
    Rel = list()
    Rel.append(self.Quant[n+k])
    Rel.append(self.TC[n+k])
    return Rel
#
#  Calculate the Cost delivered  to Demand Node i from Supply Node j
#  Adjusting for transportation losses
#
  def MarginalCost(self, Ns, Nd, Q):
    Loss = 0.0
    n = 0
#
#  Find correct cost table for Demand node i and Supply node j
#
    nc = 0
    i = 0
    while i <= Nd:
        j = 0
        while j <= Ns:
            n = i*self.NumSupplys + j
            j += 1
        i += 1
    for i in range (0, n):
        nc += self.NumPoints[i]
#
#  Need to calculate cost, then return it as a value to main functions
#
    k = nc
    while self.Quant[k] < Q:
        k += 1
    slope = (self.TC[k] - self.TC[k-1])/(self.Quant[k] - self.Quant[k-1])
    Cost = self.TC[k-1] + (Q - self.Quant[k-1])*slope
    return Cost
#
#-----Define the function for returning the integrated transportation cost-----
#
  def IntegratedCost(self, n, q):
    Cost = 0.0
    i = 0
    nc = 0
    while i < n:
        nc += self.NumPoints[i]
        i += 1
    k = nc
    while self.Quant[k] < q: k += 1
    slope = (self.TIc[k] - self.TIc[k-1])/(self.Quant[k] - self.Quant[k-1])
    Cost = self.TIc[k-1] + (q - self.Quant[k-1])*slope
    return Cost
#
#  End of TransportationCost Class
#
class TransportationLosses():
#-------------------------------------------------------------------------
#
#  This class represents relationship between moving a quantity of water from
#  supply node i to demand node j.  These losses can be consumptive
#  in nature if they are associated with riparian vegetation, or out
#  of basin flow.  Or they can be unpriced supplies to another water
#  source, which would be the case for return flows or recharge to
#  groundwater due to canal seepage or excess irrigation water
#
  def __init__(self, Np, Ns, Nd, x, y):
    self.NumSupplys = Ns
    self.NumDemands = Nd
    self.NumPoints = list()
    self.Quant = list()
    self.Loss = list()
    nc = 0
    for i in range (0, Nd):
        for j in range (0, Ns):
            self.NumPoints.append(Np[i*Ns+j])
            for k in range(0,Np[i*Ns+j]):
              self.Quant.append(x[nc+k])
              self.Loss.append(y[nc+k])
            nc += Np[i*Ns+j]
#
#  End initialization of TransportationLosses Class
#
#  Define the upper limit for the flow rate between Supply node j and
#  Demand node i
#
  def Limit(self,n):
    nc = 0
    i = 0
    while i <= n:
        nc += self.NumPoints[i]
        i += 1;
    return self.Quant[nc - 1]
#
#  Define the number of points for each transportation link
#
  def TransPoints(self, i):
    return self.NumPoints[i]

  def TransQuantRelationship(self, n, k):
    Rel = list()
    Rel.append(self.Quant[n+k])
    Rel.append(self.Loss[n+k])
    return Rel
#
#  Calculate the Quantity delivered  to Demand Node i from Supply Node j
#  Adjusting for transportation losses
#
  def TransLoss(self, Ns, Nd, Q):
    Loss = 0.0
    n = 0
#
#  Find correct loss table for Demand node i and Supply node j
#
    nc = 0
    i = 0
    while i <= Nd:
        j = 0
        while j <= Ns:
            n = i*self.NumSupplys + j
            j += 1
        i += 1
    for i in range (0, n):
        nc += self.NumPoints[i]
#
#  Need to calculate loss, then return it as a value to main functions
#
    k = nc
#    print 'Ns, Nd, nc, k, Q = ', Ns, Nd, nc, k, Q
    while (self.Quant[k] < Q):
        k += 1
    slope = (self.Loss[k] - self.Loss[k-1])/(self.Quant[k] - self.Quant[k-1])
    Cost = self.Loss[k-1] + (Q - self.Quant[k-1])*slope
    return Cost
#
#  Calculate the quantity needed from Supply Node j to Demand Node i
#  adjusting for transportation losses
#
  def TransInverse(self, Ns, Nd, Q):
    Inv = 0.0
    n = 0
    nc = 0
    i = 0
    while i <= Nd:
        j = 0
        while j <= Ns:
            n = i*self.NumSupplys + j
            j += 1
        i += 1
    for i in range (0, n):
        nc += self.NumPoints[i]
#
#  Need to calculate inverse of loss
#
    k = nc
    while self.Loss[k] < Q: k += 1
    if (self.Loss[k] > 0.0):
      slope = (self.Quant[k] - self.Quant[k-1])/(self.Loss[k] - self.Loss[k-1])
      Cost = self.Quant[k-1] + (Q - self.Loss[k-1])*slope
    else: Cost = 0.0
    return Cost
#-------------------------------------------------------------------------
#
#  The ObjectiveFunction is computed by integrating the sum of the
#  difference between the Demand Functions and the Supply Functions
#  Eventually this routine should be modified to have the marginal
#  supply and demand parameters defined as lists of variables associated
#  with each function.  This would allow for a more generic solution
#  of the problem, where different functional forms for the supply and
#  demand relationships could be used without further need to change the
#  arguments passed to this routine, as well as the numerical itegration
#  routine.  Alternatively, the supply and demand function parameters could
#  be defined as global variables, so that only the Qs values (DVs) would
#  have to be passed to the routine.
#
#-------------------------------------------------------------------------
def ObjectiveFunction(Sn, Dn, Q):
      weight = -100.0
      value = 0.0
#
#  Calculate Total Cost of water for each supply node
#
      OF = 0.0
      for i in range (0, Sn):
          Qs = 0.0
          for j in range (0, Dn):
              k = i + j*Sn
              Qs += Q[k]
#
#  Calculate the Transportation Cost associated with delivering water
#  from Supply node i to Demand node j
#
              OF -= TC.IntegratedCost(k, Q[k])
          OF -= MCS.IntegratedCost(i,Qs)
#      for i in range (0, Dn):
#          for j in range (0, Sn):
#            k = i*Sn + j
#            OF -= TC.IntegratedCost(k,Q[k])
#
#  Calculate Total Benefits of water for each demand node
#
      for i in range(0,Dn):
          Qd = 0.0
          for j in range(0,Sn):
#
#  Calculate the total demand at node i by summing all of the demands
#  associated with supply j, minus the transportation loss from j to i
#
              k = i*Sn+j
              Qd += TL.TransLoss(j, i, Q[k])
#
#  Integrate the Marginal Cost Function for the total demand at node i
#
          OF += MCD.IntegratedCost(i, Qd)
      return OF
#
#  End ObjectiveFunction definition
#-------------------------------------------------------------------------
#
#  Routine to solve an N X N set of linear equations
#
def LinearEquationSolver(N, C, y):
#
#  size is the number of rows and columns in matrix C
#  y contains size values, and is the right hand vector
#  for the linear system of equations [C]{x} = <y>
#
    x = list()
    ys = list()
    k = 0
    MaxIt = 100
    maxdel = 1.0
    DelLimit = 0.0000001
    k = 0
    for i in range (0, size):
        x.append(0.0)
        ys.append(0.0)
#
#  Solve the system using an iterative approach by computing
#  Each x value as a 2X2 matrix solution
#
    while k < MaxIt and maxdel > DelLimit:
        maxdel = 0.0
        for i in range (0, size-1):
            sum0 = y[i]
            sum1 = y[i+1]
            for j in range (0, i):
                sum0 -= C[i*N+j]*x[j]
                sum1 -= C[(i+1)*N+j]*x[j]
            for j in range (i+2, size):
                sum0 -= C[i*N+j]*x[j]
                sum1 -= C[(i+1)*N+j]*x[j]
            num = C[(i+1)*(N+1)]*sum0-C[N*i+i+1]*sum1
            den = C[(i+1)*(N+1)]*C[i*(N+1)]-C[i*N+i+1]*C[N*(i+1)+i]
            xnew = num/den
            delx = xnew - x[i]
            x[i] = xnew
            if abs(delx) > maxdel: maxdel = abs(delx)
        sum0 = y[N-2]
        sum1 = y[N-1]
        for j in range (0, N-2):
            sum0 -= C[(N-2)*N+j]*x[j]
            sum1 -= C[(N-1)*N+j]*x[j]
        num = C[(N-1)*N+N-2]*sum0-C[(N-2)*N+N-2]*sum1
        den = C[(N-1)*N+N-2]*C[(N-2)*N+N-1] - C[(N-2)*N+N-2]*C[(N-1)*N+N-1]
        xnew = num/den
        delx = xnew - x[size-1]
        x[size-1] = xnew
        if abs(delx) > maxdel: maxdel = abs(delx)
        if maxdel < DelLimit: k = MaxIt
        k += 1
#        print " k = ", k, "maxdel = ", maxdel
#        print " x = ", x
#
#   As a check, compute the new right hand vector using the iteratively solved
#   vector of x
#
#    print "Check on solved values"
#    print "      y          ys"
#    for i in range (0, size):
#        for j in range (0, size):
#            ys[i] += C[i*size+j]*x[j]
#        print y[i], ys[i]
#
#  return the vector x
#
    return x
#
#  End routine
#
#-------------------------------------------------------------------------
#
import math
import random
import array
#
#  Set increments for numerical integration, derivatives and
#  and convergence tolerance
#  Important Note:  Routines use numerical integration and derivatives
#                   which requires that
#                     deltai < deltad < tolerance
#                   If this rule is not followed, solver will not
#                   produce usable results
#
deltai = 0.005
deltad = 0.01
tolerance = 10.0
MaxIt = 500
#--------------------------------------------------------------------------
#  Generate the number of demand and supply nodes
#--------------------------------------------------------------------------

DNodes = 3
SNodes = 2

#--------------------------------------------------------------------------
#  Generate the tables for each demand-supply relationships
#  Currently, marginal cost curves for supply and demand are created
#  using tables that represent the functions with x and y coordinates
#  x = quantity and y = price
#--------------------------------------------------------------------------

print 'New Problem'
NPs = list()
Xs = list()
Ys = list()
#
# -------------------Marginal Supply Curve 1------------------------
#
#  Define the shape of the marginal cost curve for each supply node
#  Using a table of X and Y coordinates
#
Xt = [0.0, 2000.0, 4000.0, 5000.0, 7500.0, 10000.0, 20000.0, 30000.0, 50000.0, 65000.0]
Yt = [3.0, 3.061, 3.122, 3.154, 3.234, 3.316, 3.664, 4.050, 4.946, 5.747]
N = len(Xt)
for i in range (0, N):
    Xs.append(Xt[i])
    Ys.append(Yt[i])
np = len(Xt)
NPs.append(np)
#
# -------------------Marginal Supply Curve 2------------------------
#
#  Define the shape of the marginal cost curve for each supply node
#  Using a table of X and Y coordinates
#
Xt = [0.0, 1000.0, 2500.0, 5000.0, 8000.0, 10000.0, 15000.0, 25000.0]
Yt = [1.50, 1.546, 1.617, 1.743, 1.907, 2.025, 2.352, 3.176]
N = len(Xt)
for i in range (0, N):
    Xs.append(Xt[i])
    Ys.append(Yt[i])
np = len(Xt)
NPs.append(np)
#
#-------------------Assign Cost Functions----------------------
#
print 'Supply Cost Relationships'
print NPs
#print Xs
#print Ys
MCS = SupplyNodes(SNodes,NPs,Xs,Ys)
SCost = list()
SICost = list()
nc = 0
for i in range (0, SNodes):
    for j in range (0, NPs[i]):
        SCost.append(MCS.MarginalCost(i,Xs[j+nc]))
        SICost.append(MCS.IntegratedCost(i,Xs[j+nc]))
#        print i, j, j+nc, Xs[j+nc], SCost[j+nc], SICost[j+nc]
    nc += NPs[i]
#print SCost
#print SICost
#
#  End of defining Marginal Supply Curves
#
NPd = list()
Xd = list()
Yd = list()
Cond = list()
#
# -------------------Marginal Demand Curve 1 ------------------------
#
#  Define the shape of the marginal cost curve for each demand node
#  Using a table of X and Y coordinates
#
Xt = [0.0, 2000.0, 4000.0, 7500.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0]
Yt = [15.0, 12.782, 10.892, 8.232, 6.740, 4.518, 3.028, 1.361, 0.611]
N = len(Xt)
for i in range(0, N):
    Xd.append(Xt[i])
    Yd.append(Yt[i])
np = len(Xt)
NPd.append(np)
#
# -------------------Marginal Demand Curve 2 ------------------------
#
#  Define the shape of the marginal cost curve for each demand node
#  Using a table of X and Y coordinates
#
Xt = [0.0, 3000.0, 7500.0, 12500.0, 20000.0, 30000.0]
Yt = [10.0, 8.607, 6.873, 5.353, 3.679, 2.231]
N = len(Xt)
for i in range(0, N):
    Xd.append(Xt[i])
    Yd.append(Yt[i])
np = len(Xt)
NPd.append(np)
#
# -------------------Marginal Demand Curve 3 ------------------------
#
#  Define the shape of the marginal cost curve for each supply node
#  Using a table of X and Y coordinates
#
Xt = [0.0, 2500.0, 5000.0, 10000.0, 15000.0, 25000.0, 35000.0]
Yt = [25.0, 19.470, 15.163, 9.197, 5.578, 2.052, 0.755]
N = len(Xt)
for i in range(0, N):
    Xd.append(Xt[i])
    Yd.append(Yt[i])
np = len(Xt)
NPd.append(np)
#
#
#  End of defining Marginal Demand Curves
#
print 'Demand Cost Relationships'
print NPd
#print Xd
#print Yd
MCD = DemandNodes(DNodes,NPd,Xd,Yd)
DCost = list()
DICost = list()
nc = 0
for i in range (0, DNodes):
    for j in range (0, NPd[i]):
        DCost.append(MCD.MarginalCost(i,Xd[j+nc]))
        DICost.append(MCD.IntegratedCost(i,Xd[j+nc]))
#        print i, j, j+nc, Xd[j+nc], DCost[j+nc], DICost[j+nc]
    nc += NPd[i]
#print DCost
#print DICost
#--------------------------------------------------------------------------
#
#  Define the marginal cost of transportation of moving water between each
#  Supply and Demand node.  These are enterred as a table of x and y
#  relationships, with Xt = Quantity of water being provide by supply node
#  i to demand node j, and Yt = the marginal cost associated with transporting
#  water from supply node it to demand node j.  The number of points defining
#  this table can be as few as two, with no upper limit
#
#--------------------------------------------------------------------------
Xtc = list()
Ytc = list()
NPtc = list()
#
#  Transportation Cost Relationship between Demand Node 1 and Supply Node 1.
#
Xt = [0.0, 40000.0]
Yt = [1.5, 1.50]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
#
#  Transportation Cost Relationship between Demand Node 1 and Supply Node 2.
#
Xt = [0.0, 20000.0]
Yt = [0.75, 0.75]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
#
#  Transportation Cost Relationship between Demand Node 2 and Supply Node 1.
#
Xt = [0.0, 43000.0]
Yt = [1.25, 1.25]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
#
#  Transportation Cost Relationship between Demand Node 2 and Supply Node 2.
#
Xt = [0.0, 50000.0]
Yt = [100.0, 100.0]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
#
#  Transportation Cost Relationship between Demand Node 3 and Supply Node 1.
#
Xt = [0.0, 50000.0]
Yt = [100.0, 100.0]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
#
#  Transportation Cost Relationship between Demand Node 3 and Supply Node 2.
#
Xt = [0.0, 17500.0]
Yt = [1.30, 1.30]
for i in range(0,len(Xt)):
    Xtc.append(Xt[i])
    Ytc.append(Yt[i])
np = len(Xt)
NPtc.append(np)
TC = TransportationCosts(NPtc,SNodes,DNodes,Xtc,Ytc)
#
print 'Transportation Cost Relationships'
print NPtc
#print Xtc
#print Ytc
TCost = list()
TICost = list()
nc = 0
for i in range (0, DNodes):
    for j in range (0, SNodes):
        n = i*SNodes + j
        for k in range (0, NPtc[n]):
          TCost.append(TC.MarginalCost(j,i,Xtc[k+nc]))
          TICost.append(TC.IntegratedCost(n,Xtc[k+nc]))
#          print i, j, k, k+nc, Xtc[k+nc], TCost[k+nc], TICost[k+nc]
        nc += NPtc[n]
#print TCost
#print TICost
#
#--------------------------------------------------------------------------
#
#  Define the relationships for the transportation losses between each
#  Supply and Demand node.  These are enterred as a table of x and y
#  relationships, with Xt = Quantity of water being provided by supply
#  node i, and Yt = the Quantity of water received by demand node j, from
#  supply node i.  The number of points defining this table can be as few
#  as two, with no upper limit
#
#--------------------------------------------------------------------------
Xtr = list()
Ytr = list()
NPtr = list()
#
#  Transportation Loss Relationship between Demand Node 1 and Supply Node 1.
#
Xt = [0.0, 5000.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0]
Yt = [0.0, 2661.0, 8147.0, 13753.0, 19134.0, 29426.0, 39487.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
#
#  Transportation Loss Relationship between Demand Node 1 and Supply Node 2.
#
Xt = [0.0, 2500.0, 5000.0, 10000.0, 15000.0, 20000.0]
Yt = [0.0, 0.0, 1390.0, 6275.0, 11866.0, 17515.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
#
#  Transportation Loss Relationship between Demand Node 2 and Supply Node 1.
#
Xt = [0.0, 5000.0, 10000.0, 17500.0, 25000.0, 35000.0, 43000.0]
Yt = [0.0, 1467.0, 5821.0, 13959.0, 22448.0, 33443.0, 41917.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
#
#  Transportation Loss Relationship between Demand Node 2 and Supply Node 2.
#
Xt = [0.0, 50000.0]
Yt = [0.0, 0.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
#
#  Transportation Loss Relationship between Demand Node 3 and Supply Node 1.
#
Xt = [0.0, 50000.0]
Yt = [0.0, 0.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
#
#  Transportation Loss Relationship between Demand Node 3 and Supply Node 2.
#
Xt = [0.0, 2500.0, 5000.0, 7500.0, 10000.0, 15000.0, 17500.0]
Yt = [0.0, 303.0, 1717.0, 3707.0, 6071.0, 11403.0, 14209.0]
for i in range(0,len(Xt)):
    Xtr.append(Xt[i])
    Ytr.append(Yt[i])
np = len(Xt)
NPtr.append(np)
print 'Transportation Loss Relationships'
print NPtr
TL = TransportationLosses(NPtr,SNodes,DNodes,Xtr,Ytr)
TLoss = list()
nc = 0
for i in range (0, DNodes):
    for j in range (0, SNodes):
        n = i*SNodes + j
        for k in range (0, NPtr[n]):
          TLoss.append(TL.TransLoss(j,i,Xtr[k+nc]))
#          print i, j, k, k+nc, Xtr[k+nc], Ytr[k+nc], TLoss[k+nc]
        nc += NPtr[n]
#print TLoss
#--------------------------------------------------------------------------
#  The 'Matrix' of supply functions is created and stored as
#  Supply[j,i] in the list type. This represents the Marginal Supply curve
#  for Supply node j to Demand node i
#--------------------------------------------------------------------------

#----------------------------------------------------------------------------
#
#  Need to generate initial guess at Quantity from each supply node
#  to each demand node as starting guesses for 'optimal' decision variables
#
#----------------------------------------------------------------------------
Quant = list()
QuantD = list()
print " Start Iterations "
Quant =  [10000.0, 10000.0, 10000.0, 0.0, 0.0, 10000.0]
#Quant = [12000.0, 14000.0, 6000.00, 0.00, 0.00, 12000.0]
for i in range(0, DNodes):
    for j in range (0, SNodes):
        k = i*SNodes + j
        QuantD.append(TL.TransLoss(j, i, Quant[k]))
print "Initial Guess = ", Quant
OF = ObjectiveFunction(SNodes, DNodes, Quant)
print "Quantity = ", Quant, ": Objective Function = ", OF
#----------------------------------------------------------------------------
#
#  Perform iterative colculations using a while loop construction
#  This is the heart of the non-linear function optimizer
#  Current routines use a simplified first derivative search
#  approach, with a dampening factor to adjust the decision variables
#  during each iteration
#
#-------
delQ1 = list()
delQ2 = list()
delQ12 = list()
dOFdQ = list()
dOFdQ2 = list()
d2OFdQ2 = list()
dQ = list()
kk = 0
for i in range (0,DNodes*SNodes):
      dQ.append(0.0)
      delQ1.append(0.0)
      delQ2.append(0.0)
      delQ12.append(0.0)
      dOFdQ.append(0.0)
      dOFdQ2.append(0.0)
      for j in range (0,DNodes*SNodes):
           d2OFdQ2.append(0.0)

Delta = 1000.0
dampen = 1000.0
while Delta > tolerance and kk < MaxIt:
#
#    Compute the numerical derivates for the Objective Function with
#    respect to each decision variable:
#    The Objective Function is the integral of the marginal demand
#    functions from zero to the Quantity provided at each demand node minus
#    the integral of the marginal cost functions from zero to the Quantity
#    provided from supply node j to demand node i
#    The DVs are the array of quantity from supply node j to
#    demand node i
#
#  First initialize the arrays that are used for computing the
#  first and second derivatives of the Objective function with respect
#  to the water delivered from supply to demand nodes
#
    OF = ObjectiveFunction(SNodes, DNodes, Quant)
#    print "k = ", kk, "deltad = ", deltad, "Objective Function = ", OF
#    print "Quantity = ", Quant
    for i in range (0,DNodes*SNodes):
          delQ1[i] = Quant[i]
          delQ2[i] = Quant[i]
          delQ12[i] = Quant[i]
    size = DNodes*SNodes
    for i in range(0, DNodes*SNodes):
#
#    Compute the first derivate of the objective function with respect to
#    each of the decision variables (flows between Supply Node i and Demand
#    Node j
#
        delQ1[i] -= deltad
        delQ12[i] -= deltad
        OF1 = ObjectiveFunction(SNodes, DNodes, delQ1)
        dOFdQ[i] = ((OF - OF1)/deltad)
        for j in range (0, i):
            delQ2[j] -= deltad
            delQ12[j] -= deltad
            OF2 = ObjectiveFunction(SNodes, DNodes, delQ2)
            OF12 = ObjectiveFunction(SNodes, DNodes, delQ12)
            dOFdQ2[j] = (OF2 - OF12)/deltad
            d2OFdQ2[i*size+j] = (dOFdQ[i] - dOFdQ2[j])/deltad
            delQ2[j] = Quant[j]
            delQ12[j] = Quant[j]
        for j in range (i+1, DNodes*SNodes):
            delQ2[j] -= deltad
            delQ12[j] -= deltad
            OF2 = ObjectiveFunction(SNodes, DNodes, delQ2)
            OF12 = ObjectiveFunction(SNodes, DNodes, delQ12)
            dOFdQ2[j] = (OF2 - OF12)/deltad
            d2OFdQ2[i*size+j] = (dOFdQ[i] - dOFdQ2[j])/deltad
            delQ2[j] = Quant[j]
            delQ12[j] = Quant[j]
#
#  Determine the second derivative on the diagonal
#
        delQ1[i] += 2.0*deltad
        OF2 = ObjectiveFunction(SNodes, DNodes, delQ1)
        dOFdQ2[i] = (OF2 - OF)/deltad
        d2OFdQ2[i*size+i] = (dOFdQ2[i] - dOFdQ[i])/deltad
#
#  Use the Marquard Algorithm to Condition the matrix
#
        d2OFdQ2[i*size+i] += math.exp(float(kk - MaxIt)*deltad)
        delQ2[i] = Quant[i]
        delQ1[i] = Quant[i]
        delQ12[i] = Quant[i]
    kk += 1
#
#  Determine the Change in Quantity from Supply Node i to Demand Node j
#  by solving the linear set of equations [d2OFdQ2]{dQ} = {dOFdQ}
#  using the LinearEquationSolver Routine
#
#    print " [d2OFdQ2]*{dQ} = <dOFdQ> "
#    for i in range (0, SNodes*DNodes):
#        for j in range (0, SNodes*DNodes): print d2OFdQ2[i*size+j],
#        print dOFdQ[i]
    dQ = LinearEquationSolver(size, d2OFdQ2, dOFdQ)
#
#  Check on computed changes
#
    print " Link  Quantity     and   Change "
    for i in range (0, DNodes*SNodes):
       print i, Quant[i], dQ[i]
#
#  The partial derivates of the Objective Function with respect to the
#  Quantity provided from supply node j to demand node i are the
#  "direction" that decision variables should be adjusted for the simple
#  non-linear solver routine.  The "distance" the decision variables should
#  be adjusted is assumed to be 1.0.  If the convergence metric (Delta)
#  increases from one iteration to the next, a dampening coefficient will
#  be used to adjust the "distance" to accelerate convergence
#  In addition, the current routine does not place bounds on the decision
#  variables.  Routines need to be added to prevent the QuantS from containing
#  negative values.  Additional routines could be put in place to bound
#  QuantS with maximum values also.
#
    sum = 0.0
    for i in range(0,DNodes*SNodes):
        sum += dQ[i]*dQ[i]
        sum2 = 0.0
        Quant[i] += dQ[i]
    Delta1 = math.sqrt(sum)
#    dadj = dampen
#    if Delta1 > Delta: dadj = dampen*(Delta/Delta1)
#
#  Need to determine the Quantity delivered to the Demand nodes by adjusting
#  for transportation losses from the supply nodes
#
#  Determine if all Supply, Demand and Transportation flows are positive and
#  are less than the flow or use limits.
#  If they are, no adjustments, if they aren't, set to zero or max value
#
    for i in range (0, DNodes):
        for j in range (0, SNodes):
            k = i*SNodes + j
            if (Quant[k] < 0.0): Quant[k] = 0.0
            else:
                if (Quant[k] > TL.Limit(k)): Quant[k] = TL.Limit(k)
#
#  Calculate the total demand at node i by summing all of the demands
#  associated with supply j, minus the transportation loss from j to i
#
    for i in range(0, DNodes):
      for j in range(0, SNodes):
          k = i*SNodes + j
          QuantD[k] = TL.TransLoss(j, i, Quant[k])
#
#  Determine if all Supply to Demand flows are positive and are less than
#  Maximum flow rates between Supply node j and Demand node i
#  If they are, no adjustments, if they aren't, set to zero or max value
#
    for i in range (0, DNodes):
        for j in range (0, SNodes):
            k = i + j*SNodes
            if (Quant[k] < 0.0): Quant[k] = 0.0
            else:
                if (Quant[k] > TL.Limit(k)): Quant[k] = TL.Limit(k)
#
#  Determine if Demand Node Constraints are violated
#  If they are, redistribute flows proportionally from Supplies
#
    for i in range (0, DNodes):
        qd = 0.0
        for j in range (0, SNodes):
            qd += QuantD[i+j*SNodes]
        if (qd > MCD.Limit(i)):
            ratio = MCD.Limit(i)/qd
            for j in range (0, SNodes):
                QuantD[i+j*SNodes] = QuantD[i+j*SNodes]*ratio
#
#  Adjust the Supply Quantities to match the adjusted Demand Quantities
#
    for i in range (0, DNodes):
        for j in range (0, SNodes):
            k = i*SNodes + j
            Quant[k] = TL.TransInverse(j, i, QuantD[k])
#
#  Determine if Supply Node Constraints are violated
#  If they are, redistribute flow proportionally between Demand Nodes
#
    for i in range (0, SNodes):
        qs = 0.0
        for j in range (0, DNodes):
            qs += Quant[i+SNodes*j]
        if (qs > MCS.Limit(i)):
            ratio = 0.9999*MCS.Limit(i)/qs
            for j in range (0, DNodes):
                qt = Quant[i+SNodes*j]
                Quant[i+j*SNodes] = qt*ratio
    print "Quant = ", Quant
    print "QuantD = ", QuantD
#
#  Calculate new objective function using updated Quantities of water delivered
#
    OF = ObjectiveFunction(SNodes, DNodes, Quant)
#
#  Adjust change in decision variables
#
#    Delta = Delta1*dampen
    print ' k = ', kk, ' Delta = ', Delta, ' OF = ', OF
#
#  These are calculation checking routines
#  Remove after code is thoroughly checked
#
#    print 'k = ', k, 'OF = ', OF
#    print 'Delta = ', Delta
#    for i in range(0,DNodes):
#        mcd = 0.0
#        qd = 0.0
#        for j in range (0,SNodes):
#          QL = TL.TransLoss(j, i, Quant[i+j*SNodes])
#          print 'Quantity from Supply Node ', j+1, 'to Demand Node ', i+1, ' = ', Quant[j*SNodes+i], QL
#          qd += QL
#        mcd = MCD.MarginalCost(i,qd)
#        print 'Node ', i+1, ' Total Demand = ', qd, ' Marginal Demand Cost = ', mcd
#        CSurplus = NumericalIntegrationF(1, i, qd)
#        print ' Consumer Surplus for node ', i + 1, ' = ', CSurplus
#    for i in range (0, SNodes):
#        mcs = 0.0
#        qs = 0.0
#        for j in range (0, DNodes):
#            qs += Quant[j*SNodes+i]
#        mcs = MCS.MarginalCost(i,qs)
#        print ' Marginal Supply Cost for node ', i+1, ' = ', mcs
#        PSurplus = NumericalIntegrationF(0, i, qs)
#        print 'Node ', i+1, ' Total Supply = ', qs, ' Producer Surplus = ', PSurplus
















