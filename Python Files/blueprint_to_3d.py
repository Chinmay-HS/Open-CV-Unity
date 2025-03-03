# import cv2
# import numpy as np
# import pytesseract
# from ultralytics import YOLO
#
# # Load Blueprint
# image_path = "blueprint.jpg"
# image = cv2.imread(image_path)
# gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
#
# # Detect Walls (Edge Detection)
# edges = cv2.Canny(gray, 50, 150)
# contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
# wall_coordinates = [(pt[0][0], pt[0][1]) for cnt in contours for pt in cnt]
#
# # Extract Room Labels (OCR)
# ocr_result = pytesseract.image_to_data(gray, output_type=pytesseract.Output.DICT)
# room_data = [(ocr_result["text"][i], ocr_result["left"][i] + ocr_result["width"][i] // 2,
#               ocr_result["top"][i] + ocr_result["height"][i] // 2)
#              for i in range(len(ocr_result["text"])) if len(ocr_result["text"][i]) > 2]
#
# # AI Model to Detect Doors & Windows
# model = YOLO("yolov8n.pt")  # Free pretrained YOLO model
# results = model(image)
# door_window_data = [(int(box[0]), int(box[1]), int(box[2]), int(box[3])) for box in results.xyxy[0].cpu().numpy()]
#
# # Save Data for Unity
# with open("house_model.txt", "w") as f:
#     for coord in wall_coordinates:
#         f.write(f"WALL {coord[0]} {coord[1]}\n")
#     for room in room_data:
#         f.write(f"ROOM {room[0]} {room[1]} {room[2]}\n")
#     for door_window in door_window_data:
#         f.write(f"DOOR_WINDOW {door_window[0]} {door_window[1]} {door_window[2]} {door_window[3]}\n")
#
# print("Blueprint processed & saved to house_model.txt!")
import cv2
import numpy as np
import os
import json


class SimplifiedBlueprintProcessor:
    def __init__(self, image_path="blueprint.jpg"):
        self.image_path = image_path
        self.rooms = {}  # Dictionary to store room coordinates

        # Check if image exists
        if not os.path.exists(image_path):
            print(f"Error: {image_path} not found!")
            exit()

        # Load image
        self.original = cv2.imread(image_path)
        self.gray = cv2.imread(image_path, cv2.IMREAD_GRAYSCALE)

        # Resize for consistency
        self.original = cv2.resize(self.original, (1200, 900))
        self.gray = cv2.resize(self.gray, (1200, 900))

    def detect_walls(self):
        # Apply preprocessing for wall detection
        blurred = cv2.GaussianBlur(self.gray, (5, 5), 0)
        _, binary = cv2.threshold(blurred, 180, 255, cv2.THRESH_BINARY_INV)

        # Apply morphological operations to enhance walls
        kernel = np.ones((3, 3), np.uint8)
        dilated = cv2.dilate(binary, kernel, iterations=2)
        eroded = cv2.erode(dilated, kernel, iterations=1)

        # Find contours (possible walls)
        contours, _ = cv2.findContours(eroded, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

        # Filter contours by size to remove noise
        filtered_contours = []
        for cnt in contours:
            area = cv2.contourArea(cnt)
            if area > 100:  # Adjust threshold as needed
                filtered_contours.append(cnt)

        # Simplify contours to reduce point count
        wall_contours = []
        for cnt in filtered_contours:
            epsilon = 0.01 * cv2.arcLength(cnt, True)
            approx = cv2.approxPolyDP(cnt, epsilon, True)
            wall_contours.append(approx)

        self.wall_contours = wall_contours
        return wall_contours

    def detect_rooms_by_clustering(self):
        # Detect rooms by analyzing the wall contours
        # We'll define rooms based on enclosed spaces
        for i, contour in enumerate(self.wall_contours):
            area = cv2.contourArea(contour)
            if area > 1000:  # Only consider larger areas as rooms
                # Create a mask for this contour
                mask = np.zeros_like(self.gray)
                cv2.drawContours(mask, [contour], 0, 255, -1)

                # Create a default room name based on size
                if area > 40000:
                    room_name = f"LivingRoom_{i}"
                elif area > 20000:
                    room_name = f"Bedroom_{i}"
                elif area > 10000:
                    room_name = f"Kitchen_{i}"
                elif area > 5000:
                    room_name = f"Bathroom_{i}"
                else:
                    room_name = f"SmallRoom_{i}"

                self.rooms[room_name] = contour

        return self.rooms

    def manually_add_room(self, room_name, contour_points):
        """
        Allows manual addition of room data
        contour_points should be a list of [x,y] points
        """
        contour = np.array(contour_points, dtype=np.int32).reshape((-1, 1, 2))
        self.rooms[room_name] = contour

    def generate_output_files(self):
        # Create output folder if it doesn't exist
        if not os.path.exists("blueprint_data"):
            os.makedirs("blueprint_data")

        # Save wall coordinates
        with open("blueprint_data/walls.txt", "w") as f:
            for contour in self.wall_contours:
                for point in contour:
                    x, y = point[0]
                    f.write(f"{x} {y}\n")
                f.write("NEXT_CONTOUR\n")  # Separator between contours

        # Save room information
        with open("blueprint_data/rooms.txt", "w") as f:
            for room_name, contour in self.rooms.items():
                f.write(f"ROOM:{room_name}\n")
                for point in contour:
                    x, y = point[0]
                    f.write(f"{x} {y}\n")
                f.write("END_ROOM\n")

        # Create visualization
        vis_image = self.original.copy()

        # Draw walls
        cv2.drawContours(vis_image, self.wall_contours, -1, (0, 255, 0), 2)

        # Label rooms
        for room_name, contour in self.rooms.items():
            M = cv2.moments(contour)
            if M["m00"] != 0:
                cx = int(M["m10"] / M["m00"])
                cy = int(M["m01"] / M["m00"])
                cv2.putText(vis_image, room_name, (cx, cy),
                            cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)

        # Save visualization
        cv2.imwrite("blueprint_data/visualization.jpg", vis_image)

        return vis_image

    def process_blueprint(self):
        self.detect_walls()
        self.detect_rooms_by_clustering()
        vis_image = self.generate_output_files()

        cv2.imshow("Click to identify rooms", vis_image)
        print("Walls and estimated rooms have been detected.")
        print("Use the visual interface to manually add or correct room names:")
        print("1. View the initial room detection in the window")
        print("2. Press any key to close the preview")
        cv2.waitKey(0)
        cv2.destroyAllWindows()

        return vis_image

    def interactive_room_labeling(self):
        """
        Opens a simple interface for the user to label rooms
        """
        display_img = self.original.copy()
        cv2.drawContours(display_img, self.wall_contours, -1, (0, 255, 0), 2)

        def click_event(event, x, y, flags, param):
            if event == cv2.EVENT_LBUTTONDOWN:
                # Find which contour contains this point
                for idx, contour in enumerate(self.wall_contours):
                    if cv2.pointPolygonTest(contour, (x, y), False) >= 0:
                        room_name = input(f"Enter name for room at point ({x},{y}): ")
                        if room_name:
                            self.rooms[room_name] = contour
                            # Update display
                            M = cv2.moments(contour)
                            if M["m00"] != 0:
                                cx = int(M["m10"] / M["m00"])
                                cy = int(M["m01"] / M["m00"])
                                cv2.putText(display_img, room_name, (cx, cy),
                                            cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                                cv2.imshow("Label Rooms", display_img)
                        break

        cv2.imshow("Label Rooms", display_img)
        cv2.setMouseCallback("Label Rooms", click_event)
        print("Click inside a room to label it. Press ESC when done.")
        while True:
            key = cv2.waitKey(0) & 0xFF
            if key == 27:  # ESC key
                break
        cv2.destroyAllWindows()

        # Generate output files again with the new room data
        self.generate_output_files()


# Example usage with manual room labeling
if __name__ == "__main__":
    processor = SimplifiedBlueprintProcessor("blueprint.jpg")
    processor.detect_walls()

    # Option 1: Automatic room detection
    processor.detect_rooms_by_clustering()

    # Option 2: Manual room definition
    # You can manually add rooms with known coordinates if needed
    # processor.manually_add_room("MasterBedroom", [[100, 100], [100, 300], [300, 300], [300, 100]])

    # Option 3: Interactive room labeling
    processor.interactive_room_labeling()

    # Generate final output
    result = processor.generate_output_files()

    cv2.imshow("Processed Blueprint", result)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

    print("Processing complete. Files saved in 'blueprint_data' folder.")