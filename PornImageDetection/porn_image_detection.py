from nudenet import NudeClassifier
import sys

classifier = NudeClassifier()

image_path = sys.argv[1]

result = classifier.classify(image_path)
print(0 if result[image_path]['safe'] > result[image_path]['unsafe'] else 1)