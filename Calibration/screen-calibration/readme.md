# Projector and Camera Calibration

1. Set projector and camera settings according to the README file in the Calibration folder.
2. Display fullscreen white (white.bonsai).
3. If necessary, adjust projector margins and keystone to ensure projection fits the outer box area.
4. Display calibration target (target.bonsai).
5. Adjust warp corners to match the box corners, aligning each corner with the border between green and white.
6. Check that the center of the calibration target matches the physical center of the box.
7. Align camera calibration target (white cross) with the projection calibration target so that they overlap.
8. Adjust radial and tangential distortion parameters (Undistort node) so that the edges of the box look straight.
9. Define a region of interest in the Crop node such that the green border fits snug inside a rectangle.
10. Set the Resize node parameters to be half of the crop region size.
11. Scale the focal length to half (we need to do this because we resize the image by a factor of 2; this is enough because only the Resize node changes the proportions of the image, whereas the Crop only selects a subregion).
12. Update the principal point of the Undistort to match the center of the white cross in the resized image (the reason this works is that the Crop and Resize operations are modifying the calibration image such that the cross still marks the optical center of the lens).

# Undistort Parameters (original)

| Parameter          | Value        |
| ------------------ | ------------ |
| Focal length       | 2000, 2000   |
| Principal point    | 800, 600     |
| Radial distortion  | -0.3, 0, 0   |
| Tang. distortion   | 0, -0.01     |

# Undistort Parameters (resized)

| Parameter          | Value        |
| ------------------ | ------------ |
| Focal length       | 1000, 1000   |
| Principal point    | 317, 254     |
| Radial distortion  | -0.3, 0, 0   |
| Tang. distortion   | 0, -0.01     |
