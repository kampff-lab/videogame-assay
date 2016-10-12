# Poke Calibration

1. Open the poke calibration workflow (poke-calibration.bonsai).

2. Display fullscreen white (white.bonsai).
3. Align projector margins and keystone.
4. Display calibration target (target.bonsai).
5. Adjust warp corners to match the box corners, aligning each corner with the border between green and white.
6. Check that the center of the calibration target matches the physical center of the box.
7. Align camera calibration target (white cross) with the projection calibration target so that they overlap.

# Poke Testing

1. Open the poke testing workflow (poke-test.bonsai).
2. Set the PortName to the poke you want to test and run the workflow.
3. Test LED on/off.
4. Test Piezo buzzer (availability tone and reward tone).
5. Test poke sensor (physically trigger the poke).
6. Test valve (toggle water valve on/off - you should HEAR it clicking).

## Arduino Parameters

Model: Arduino Uno
Left Poke Port: COM6 @ 57600bps
Right Poke Port: COM4 @ 57600bps

| Wire               | Pin |
| ------------------ | ----|
| Poke LED           | 5   |
| Poke sensor        | 6   |
| Valve              | 2   |
| Piezo buzzer       | 3   |
