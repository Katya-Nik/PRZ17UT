package files;

public class File {
    private String extension;
    private String filename;
    private String content;
    private double size;

    /**
     * Construct object with passed filename and content, set extension based
     * on filename and calculate size as half content length.
     * @param filename files.File name (mandatory) with extension (optional), without directory tree (path separators:
     *                 https://en.wikipedia.org/wiki/Path_(computing)#Representations_of_paths_by_operating_system_and_shell)
     * @param content files.File content (could be empty, but must be set)
     */
    public File(String filename, String content) {
        this.filename = filename;
        this.content = content;
        this.size = content.length() / 2;
        this.extension = filename.split("\\.")[filename.split("\\.").length - 1];
    }

    /**
     * Get exactly file size
     * @return files.File size
     */
    public double getSize() {
        return (int) size;
    }

    /**
     * Get files.File filename
     * @return files.File filename
     */
    public String getFilename() {
        return filename;
    }
}
