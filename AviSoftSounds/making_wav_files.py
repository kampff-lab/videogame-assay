# -*- coding: utf-8 -*-


import math
import scipy.signal
from os.path import join
from pydub import AudioSegment
import pydub
import numpy as np


# CONSTANTS THE WAY AviSoft LIKES THEM
half_range_of_bit_16 = 32767
sample_rate = 1000000 


def _create_silence(audio, sample_rate=1000000, duration_milliseconds=1000):
    
    num_samples = duration_milliseconds * (sample_rate / 1000.0)

    for x in range(int(num_samples)): 
        audio.append(1) # Make sure the Least Important Bit is set to 1
    
    return audio


def _create_sinewave(audio, sample_rate=1000000, freq=2000.0, 
                     duration_milliseconds=1000, volume=1.0):

    num_samples = duration_milliseconds * (sample_rate / 1000.0)

    for x in range(int(num_samples)):
        value = int(volume * half_range_of_bit_16 * \
                     math.sin(2 * math.pi * freq * ( x / sample_rate )))
        if value % 2 == 1 and value != half_range_of_bit_16: # Make sure the Least Important Bit is set to 
            value = value + 1
        elif value == half_range_of_bit_16:
            value = value -1 
        audio.append(value)

    return audio

def _create_sinewavelet(audio, sample_rate=1000000, freq=2000.0, 
                        duration_milliseconds=1000, volume=1.0):

    num_samples = duration_milliseconds * (sample_rate / 1000.0)
    normal = scipy.signal.gaussian(num_samples, num_samples/7, True)
    
    for x in range(int(num_samples)):
        value = int(volume * half_range_of_bit_16 * \
                     math.sin(2 * math.pi * freq * ( x / sample_rate )))
        value = int(value * normal[x])
        # Make sure the Least Important Bit is set to 0 by making the value even
        # That will set the AviSoft Trigger off (it is constantly at 5V otherwise)
        if value % 2 == 1 and value != half_range_of_bit_16: 
            value = value + 1
        elif value == half_range_of_bit_16:
            value = value -1
        
        #if x > num_samples - 100:
         #   value = 0
        audio.append(value)

    return audio


def make_tone(file, channel=1, sample_rate=1000000, 
                             freq=1000, duration_milliseconds=1000, 
                             volume=1.0):
    audio = []
    channel_tone = _create_sinewave(audio, sample_rate=sample_rate, freq=freq, 
                               duration_milliseconds=duration_milliseconds, 
                               volume=volume)
    channel_tone = np.array(channel_tone,dtype=np.int16)
    audio_segment_channel_tone = pydub.AudioSegment(channel_tone.tobytes(), 
                                        frame_rate=sample_rate,
                                        sample_width=channel_tone.dtype.itemsize, 
                                        channels=1)
    audio = []
    channel_silence = _create_silence(audio, sample_rate=sample_rate, 
                               duration_milliseconds=duration_milliseconds)
    channel_silence = np.array(channel_silence,dtype=np.int16)
    audio_segment_silence = pydub.AudioSegment(channel_silence.tobytes(), 
                                        frame_rate=sample_rate,
                                        sample_width=channel_silence.dtype.itemsize, 
                                        channels=1)
    if channel==1:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_channel_tone, audio_segment_silence)
    elif channel==2:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_silence, audio_segment_channel_tone)
    elif channel==3:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_channel_tone, audio_segment_channel_tone)
        
    mutli_channel.export(file, format='wav')
    
    return channel_tone, channel_silence


def make_beeping_tone(file, channel=1, sample_rate=1000000, 
                                     freq=1000, duration_milliseconds=1000, 
                                     beeps_per_second=1, volume=1.0):
    
    total_beeps = int(np.floor(beeps_per_second * duration_milliseconds / 1000))
    
    channel_tone = []
    beep_duration_milliseconds = 0.5 * duration_milliseconds / total_beeps
    for beep in np.arange(total_beeps):
        channel_tone = _create_sinewavelet(channel_tone, sample_rate=sample_rate, freq=freq, 
                                 duration_milliseconds=beep_duration_milliseconds, 
                                 volume=volume)
        channel_tone = _create_silence(channel_tone, sample_rate=sample_rate, 
                                duration_milliseconds=beep_duration_milliseconds)
    
    channel_tone = np.array(channel_tone,dtype=np.int16)
    audio_segment_channel_tone = pydub.AudioSegment(channel_tone.tobytes(), 
                                        frame_rate=sample_rate,
                                        sample_width=channel_tone.dtype.itemsize, 
                                        channels=1)
    channel_silence = []
    silence_duration_milliseconds = 2 * beep_duration_milliseconds * total_beeps
    channel_silence = _create_silence(channel_silence, sample_rate=sample_rate, 
                                      duration_milliseconds=silence_duration_milliseconds)
    
    channel_silence = np.array(channel_silence,dtype=np.int16)
    audio_segment_silence = pydub.AudioSegment(channel_silence.tobytes(), 
                                        frame_rate=sample_rate,
                                        sample_width=channel_silence.dtype.itemsize, 
                                        channels=1)
    
    if channel==1:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_channel_tone, audio_segment_silence)
    elif channel==2:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_silence, audio_segment_channel_tone)
    elif channel==3:
        mutli_channel = AudioSegment.from_mono_audiosegments(audio_segment_channel_tone, audio_segment_channel_tone)
        
    mutli_channel.export(file, format='wav')
    
    return channel_tone, channel_silence

    


folder = r'C:\Projects\Videogame\AviSoftSounds'

'''
freq= 14000
duration_milliseconds=3000
file = join(folder, '14KHz_Channel1.wav')
make_tone(file, channel=1, sample_rate=sample_rate, freq=14000, 
                                duration_milliseconds=5000, volume=1.0)

file = join(folder, '14KHz_Channel2.wav')
make_tone(file, channel=2, sample_rate=sample_rate, freq=14000, 
                                duration_milliseconds=5000, volume=1.0)

'''



channel = 1
freq = 4000
duration_milliseconds = 20000
beeps_per_second = 4
volume = 0.75
file = join(folder, 'Availability_Left_At4Hz_Freq4KHz_20secs.wav')
channel_tone, channel_silence = make_beeping_tone(file, channel=channel, sample_rate=1000000, 
                  freq=freq, duration_milliseconds=duration_milliseconds, 
                  beeps_per_second=beeps_per_second, volume=volume)


channel = 2
freq = 12000
duration_milliseconds = 20000
beeps_per_second = 4
volume = 0.75
file = join(folder, 'Availability_Right_At4Hz_Freq12KHz_20secs.wav')
channel_tone, channel_silence = make_beeping_tone(file, channel=channel, sample_rate=1000000, 
                  freq=freq, duration_milliseconds=duration_milliseconds, 
                  beeps_per_second=beeps_per_second, volume=volume)

channel = 1
freq = 8000
duration_milliseconds = 500
beeps_per_second = 4
volume = 1
file = join(folder, 'Reward_Left_At4Hz_Freq8KHz_0p5secs.wav')
channel_tone, channel_silence = make_beeping_tone(file, channel=channel, sample_rate=1000000, 
                  freq=freq, duration_milliseconds=duration_milliseconds, 
                  beeps_per_second=beeps_per_second, volume=volume)

channel = 2
freq = 8000
duration_milliseconds = 500
beeps_per_second = 4
volume = 1
file = join(folder, 'Reward_Right_At4Hz_Freq8KHz_0p5secs.wav')
channel_tone, channel_silence = make_beeping_tone(file, channel=channel, sample_rate=1000000, 
                  freq=freq, duration_milliseconds=duration_milliseconds, 
                  beeps_per_second=beeps_per_second, volume=volume)




