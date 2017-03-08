# -*- coding: utf-8 -*-
"""
Created on Fri Mar  3 18:12:09 2017

@author: KAMPFF-LAB-BEHAVIOR1
"""

import numpy as np
import pandas as pd
import numpy.linalg as linalg
import matplotlib.pyplot as plt

def fitDistortionDotsModel(fileName):
    # Calibration image: grid of dots ranging from [-1,1] in steps of 0.05
    # Note: the first and last row/column are removed to avoid edges
    # Number of cols is: (1 / stepSize - 1 [ignore edge]) * 2 [for either side]
    x = np.arange(-19,20,1)
    sx = x * 0.05
    sy = x * 0.05
    xx, yy = np.meshgrid(sx, sy)
    screenCorners = pd.DataFrame({'x': xx.ravel(),
                                  'y': yy.ravel()})
    
    cameraCorners = pd.read_csv(fileName,
                                delimiter=' ',
                                header=None,
                                names=['x','y'],
                                usecols=[0,1])
    gs = np.array_split(cameraCorners,39) # split rows (chunks of 39)
    gs = [g.sort_values(by='x') for g in gs] # sort each row by X
    cameraCorners = pd.concat(gs) # group everything again
    # [plt.plot(g.x,g.y,'r' if i % 2 == 0 else 'b')  for i,g in enumerate(gs)]
    cameraCorners.reset_index(drop=True,inplace=True)
    
    bias = pd.DataFrame(np.ones(len(cameraCorners)))
    features = pd.concat([bias,
                          cameraCorners,
                          cameraCorners*cameraCorners,
                          cameraCorners*cameraCorners*cameraCorners],
                          axis=1)
    features.columns = ['b','x','y','x2','y2','x3','x4']
    # linear regression using least squares: y = aX + b
    return linalg.lstsq(features,screenCorners),features,screenCorners


def plotDistortionModel(X,features,screenCorners):
    # Compare distortion model predictions with ground truth
    prediction = features.dot(X)
    plt.plot(screenCorners.x,screenCorners.y,'b.')
    plt.plot(prediction[0],prediction[1],'r.')

def fitDistortionModel(fileName,stepSize=0.02):
    cameraCorners = pd.read_csv(fileName,
                                delimiter=' ',
                                header=None,
                                names=['x','y'],
                                usecols=[0,1])
    ncols = 1.0 / stepSize - 3
    x = np.arange(ncols) * stepSize + 2 * stepSize
    sx = x * 2 - 1
    sy = x * -2 + 1
    xx, yy = np.meshgrid(sx, sy)
    screenCorners = pd.DataFrame({'x': xx.ravel(),
                                  'y': yy.ravel()})
    
    bias = pd.DataFrame(np.ones(len(cameraCorners)))
    features = pd.concat([bias,
                          cameraCorners,
                          cameraCorners*cameraCorners],
                          axis=1)
    return linalg.lstsq(features,screenCorners),features,screenCorners
    