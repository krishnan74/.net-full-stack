from flask import Flask, request, jsonify
import json
import re
import nltk
from nltk.tokenize import word_tokenize
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
from difflib import SequenceMatcher

app = Flask(__name__)

# Download required NLTK data 
try:
    nltk.download('punkt_tab')  
    nltk.download('punkt')
    nltk.download('stopwords')
    nltk.download('wordnet')
except Exception as e:
    print(f"Error downloading NLTK resources: {e}")
    exit(1)

# Load FAQ data from JSON file
try:
    with open('bank_faq.json', 'r') as f:
        faq_data = json.load(f)
except FileNotFoundError:
    print("Error: bank_faq.json file not found. Please keep it in the same directory.")
    exit(1)

# Initialize NLTK tools
stop_words = set(stopwords.words('english'))
lemmatizer = WordNetLemmatizer()

def preprocess_text(text):
    text = re.sub(r'[^\w\s]', '', text.lower().strip())
    tokens = word_tokenize(text)
    tokens = [lemmatizer.lemmatize(token) for token in tokens if token not in stop_words]
    return tokens

def similarity(text1, text2):
    tokens1 = preprocess_text(text1)
    tokens2 = preprocess_text(text2)
    set1, set2 = set(tokens1), set(tokens2)
    if not set1 or not set2:
        return 0.0
    overlap = len(set1.intersection(set2))
    union = len(set1.union(set2))
    jaccard_sim = overlap / union
    seq_sim = SequenceMatcher(None, ' '.join(tokens1), ' '.join(tokens2)).ratio()
    return 0.6 * jaccard_sim + 0.4 * seq_sim

def find_relevant_faqs(user_input, faqs, threshold=0.4):
    relevant_faqs = []
    for faq in faqs:
        score = similarity(user_input, faq["question"])
        if score >= threshold:
            relevant_faqs.append((faq, score))
    relevant_faqs.sort(key=lambda x: x[1], reverse=True)
    return relevant_faqs

def combine_answers(relevant_faqs):
    if not relevant_faqs:
        return None
    if len(relevant_faqs) == 1:
        return relevant_faqs[0][0]["answer"]
    
    combined_answer = "Here's the information based on your query:\n"
    for faq, score in relevant_faqs[:2]:
        combined_answer += f"- {faq['answer']}\n"
    return combined_answer

@app.route('/chat', methods=['POST'])
def chat():
    user_input = request.json.get('question', '')
    cleaned_input = re.sub(r'[^\w\s]', '', user_input.strip())
    if not cleaned_input:
        return jsonify({"error": "Please enter a valid question."}), 400

    relevant_faqs = find_relevant_faqs(cleaned_input, faq_data["faqs"])
    if relevant_faqs:
        answer = combine_answers(relevant_faqs)
        return jsonify({"answer": answer}), 200
    else:
        return jsonify({"answer": "I'm sorry, I couldn't find specific information for your query. Please try rephrasing your question"}), 404

if __name__ == "__main__":
    app.run(debug=True)