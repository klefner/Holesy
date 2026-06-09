#!/usr/bin/env python3
"""
Extract base64-encoded MP3 audio samples from the HTML file
and save them into the Unity project's Audio folder.
"""

import re
import base64
import os

HTML_PATH = "/home/user/Holesy/downtown_hole_MASTER 3.html"
OUTPUT_DIR = "/home/user/Holesy/DowntownDevour/Assets/Audio"

# Map: (variable_name, output_prefix, is_array)
VARIABLES = [
    ("SCREAM_SAMPLES_B64",       "scream",       True),
    ("TREE_SAMPLES_B64",         "tree",         True),
    ("CAR_SAMPLES_B64",          "car",          True),
    ("BUILDING_SAMPLES_B64",     "building",     True),
    ("METAL_SAMPLES_B64",        "metal",        True),
    ("GUNSHOT_SAMPLES_B64",      "gunshot",      True),
    ("SOLDIER_VOICE_SAMPLES_B64","soldiervoice",  True),
    ("BITE_CHEW_B64",            "bite_chew",    False),
]


def extract_variable_block(text, var_name, is_array):
    """
    Extract the raw JS value for a variable (either an array [...] or a plain string).
    Returns the raw text block between the opening and closing bracket/quote.
    """
    # Find the declaration
    pattern = rf'const\s+{re.escape(var_name)}\s*='
    m = re.search(pattern, text)
    if not m:
        raise ValueError(f"Variable {var_name} not found in HTML")

    start = m.end()
    # Skip whitespace
    while start < len(text) and text[start] in ' \t\n\r':
        start += 1

    if is_array:
        if text[start] != '[':
            raise ValueError(f"Expected '[' for array {var_name}, got {text[start]!r}")
        # Find matching closing bracket
        depth = 0
        i = start
        while i < len(text):
            if text[i] == '[':
                depth += 1
            elif text[i] == ']':
                depth -= 1
                if depth == 0:
                    return text[start:i+1]
            i += 1
        raise ValueError(f"Unclosed array for {var_name}")
    else:
        # Single string — find the opening quote
        quote_char = text[start]
        if quote_char not in ('"', "'", '`'):
            raise ValueError(f"Expected quote for {var_name}, got {quote_char!r}")
        # Collect everything until the semicolon terminator, then parse
        # Find end: a semicolon or newline that ends the statement
        end = text.find(';', start)
        if end == -1:
            end = text.find('\n', start)
        return text[start:end]


def collect_b64_strings(block):
    """
    Given a JS block (array literal or string expression), extract all
    quoted string literals and concatenate adjacent ones (joined by '+').
    Returns a list of complete base64 strings (one per logical entry).
    """
    # Tokenise: find all quoted strings and '+' operators, ignoring whitespace/commas/brackets
    # We use a simple state machine to handle multi-line concatenation.

    # First, extract all quoted string tokens in order, plus note their positions
    token_re = re.compile(
        r'"((?:[^"\\]|\\.)*)"|'   # double-quoted
        r"'((?:[^'\\]|\\.)*)'|"   # single-quoted
        r'`((?:[^`\\]|\\.)*)`',   # backtick
        re.DOTALL
    )

    tokens = []
    for m in token_re.finditer(block):
        s = m.group(1) if m.group(1) is not None else (m.group(2) if m.group(2) is not None else m.group(3))
        tokens.append((m.start(), m.end(), s))

    if not tokens:
        return []

    # Now group consecutive strings that are connected only by '+' (and whitespace/newlines)
    # between their positions.
    groups = []
    current_group = [tokens[0][2]]
    prev_end = tokens[0][1]

    for i in range(1, len(tokens)):
        tok_start, tok_end, tok_str = tokens[i]
        between = block[prev_end:tok_start]
        # Strip whitespace and check if the only non-whitespace chars are '+'
        stripped = between.strip()
        if stripped == '+' or re.match(r'^(\+\s*)+$', stripped):
            # Continuation of the same logical string
            current_group.append(tok_str)
        else:
            # New entry (comma or bracket boundary)
            groups.append(''.join(current_group))
            current_group = [tok_str]
        prev_end = tok_end

    groups.append(''.join(current_group))
    return groups


def main():
    print(f"Reading HTML file...")
    with open(HTML_PATH, 'r', encoding='utf-8', errors='replace') as f:
        html = f.read()
    print(f"  File size: {len(html):,} characters")

    os.makedirs(OUTPUT_DIR, exist_ok=True)
    print(f"Output directory: {OUTPUT_DIR}")
    print()

    saved_files = []

    for var_name, prefix, is_array in VARIABLES:
        print(f"Processing {var_name}...")
        try:
            block = extract_variable_block(html, var_name, is_array)
        except ValueError as e:
            print(f"  ERROR: {e}")
            continue

        b64_strings = collect_b64_strings(block)
        print(f"  Found {len(b64_strings)} string(s)")

        if not is_array:
            # Single file
            if b64_strings:
                b64_data = b64_strings[0]
                try:
                    audio_bytes = base64.b64decode(b64_data)
                    filename = f"{prefix}.mp3"
                    out_path = os.path.join(OUTPUT_DIR, filename)
                    with open(out_path, 'wb') as f:
                        f.write(audio_bytes)
                    size_kb = len(audio_bytes) / 1024
                    print(f"  Saved: {filename} ({size_kb:.1f} KB)")
                    saved_files.append((filename, size_kb))
                except Exception as e:
                    print(f"  ERROR decoding {prefix}.mp3: {e}")
        else:
            for idx, b64_data in enumerate(b64_strings):
                if not b64_data.strip():
                    continue
                try:
                    audio_bytes = base64.b64decode(b64_data)
                    filename = f"{prefix}_{idx}.mp3"
                    out_path = os.path.join(OUTPUT_DIR, filename)
                    with open(out_path, 'wb') as f:
                        f.write(audio_bytes)
                    size_kb = len(audio_bytes) / 1024
                    print(f"  Saved: {filename} ({size_kb:.1f} KB)")
                    saved_files.append((filename, size_kb))
                except Exception as e:
                    print(f"  ERROR decoding {prefix}_{idx}.mp3: {e}")

        print()

    print("=" * 50)
    print(f"SUMMARY: {len(saved_files)} file(s) saved to {OUTPUT_DIR}")
    print("=" * 50)
    for fname, size_kb in saved_files:
        print(f"  {fname:<35} {size_kb:>8.1f} KB")

    return saved_files


if __name__ == "__main__":
    main()
