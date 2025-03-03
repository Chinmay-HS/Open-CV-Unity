import cv2
import numpy as np
import os

# Load the blueprint image
image_path = "blueprint.jpg"  # Ensure the image is in the same folder as the script
if not os.path.exists(image_path):
    print(f"Error: {image_path} not found!")
    exit()

image = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
image = cv2.resize(image, (800, 600))  # Resize for consistency

# Apply Gaussian Blur to smooth edges
blurred = cv2.GaussianBlur(image, (5, 5), 0)

# Apply Canny Edge Detection to find walls
edges = cv2.Canny(blurred, 50, 150)

# Find contours (possible walls)
contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

# Convert contours into wall coordinates
wall_coordinates = []
for cnt in contours:
    for point in cnt:
        x, y = point[0]  # Extract (x, y) coordinate
        wall_coordinates.append((x, y))

# Save the coordinates in a text file for Unity
output_file = "walls.txt"
with open(output_file, "w") as f:
    for coord in wall_coordinates:
        f.write(f"{coord[0]} {coord[1]}\n")

print(f"Wall coordinates saved in {output_file}")

# Draw contours for visualization
output_image = cv2.cvtColor(image, cv2.COLOR_GRAY2BGR)
cv2.drawContours(output_image, contours, -1, (0, 255, 0), 2)  # Draw in green

# Save and display processed image
processed_image_path = "processed_blueprint.jpg"
cv2.imwrite(processed_image_path, output_image)

cv2.imshow("Detected Walls", output_image)
cv2.waitKey(0)
cv2.destroyAllWindows()


# import cv2
# import numpy as np
# import os
#
# # Load the blueprint image
# image_path = "blueprint.jpg"  # Ensure image is in the script's folder
# if not os.path.exists(image_path):
#     print(f"Error: {image_path} not found!")
#     exit()
#
# image = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)
# image = cv2.resize(image, (800, 600))  # Resize for consistency
#
# # Apply adaptive thresholding to extract structures
# adaptive_thresh = cv2.adaptiveThreshold(image, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C,
#                                         cv2.THRESH_BINARY_INV, 11, 2)
#
# # Morphological operations to clean noise
# kernel = np.ones((3, 3), np.uint8)
# processed = cv2.morphologyEx(adaptive_thresh, cv2.MORPH_CLOSE, kernel, iterations=2)
#
# # Canny edge detection to extract edges
# edges = cv2.Canny(processed, 50, 150)
#
# # Find contours for detecting different elements
# contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
#
# wall_coordinates = []
# door_coordinates = []
# window_coordinates = []
#
# # Classify contours based on size & shape
# for cnt in contours:
#     area = cv2.contourArea(cnt)
#     if area > 2000:  # Walls (large areas)
#         for point in cnt:
#             x, y = point[0]
#             wall_coordinates.append((x, y))
#     elif 500 < area < 2000:  # Doors
#         for point in cnt:
#             x, y = point[0]
#             door_coordinates.append((x, y))
#     elif 100 < area < 500:  # Windows
#         for point in cnt:
#             x, y = point[0]
#             window_coordinates.append((x, y))
#
# # Save the coordinates for Unity processing
# def save_coordinates(filename, coordinates):
#     with open(filename, "w") as f:
#         for coord in coordinates:
#             f.write(f"{coord[0]} {coord[1]}\n")
#
# save_coordinates("walls.txt", wall_coordinates)
# save_coordinates("doors.txt", door_coordinates)
# save_coordinates("windows.txt", window_coordinates)
#
# print("Wall, Door, and Window coordinates saved!")
#
# # Draw the detected features for visualization
# output_image = cv2.cvtColor(image, cv2.COLOR_GRAY2BGR)
# cv2.drawContours(output_image, contours, -1, (0, 255, 0), 2)  # Draw in green
#
# # Save and display the processed image
# processed_image_path = "processed_blueprint.jpg"
# cv2.imwrite(processed_image_path, output_image)
#
# cv2.imshow("Detected Structures", output_image)
# cv2.waitKey(0)
# cv2.destroyAllWindows()
# sdasdasdasdasd