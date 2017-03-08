# -*- coding: utf-8 -*-
"""
Created on Tue Feb  7 17:50:50 2017

@author: glopes
"""

import numpy as np
import pandas as pd

def framedrops(counterpath):
    counter = pd.read_csv(counterpath,names=['counter'],
                          delimiter=' ',usecols=[1])
    drops = counter.diff() - 1
    return drops.counter
    
# Set the following path to be the 'video.csv' file of the folder to test
path = r'video.csv'
drops = framedrops(path)
totaldrops = drops.sum()
print(totaldrops)
if totaldrops > 0:
    print(drops[drops > 0])
    print(drops.sum() / drops.count())
