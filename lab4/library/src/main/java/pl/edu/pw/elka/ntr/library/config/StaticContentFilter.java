package pl.edu.pw.elka.ntr.library.config;

import static org.springframework.http.HttpStatus.NOT_FOUND;

import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;
import java.util.List;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Component;

@Component
public class StaticContentFilter implements Filter {
    private static final List<String> FILE_EXTENSIONS = Arrays.asList("html", "js", "css", "json", "png", "ico");

    @Override
    public void doFilter(final ServletRequest servletRequest, final ServletResponse servletResponse,
            final FilterChain filterChain) throws IOException, ServletException {
        doFilter((HttpServletRequest) servletRequest, (HttpServletResponse) servletResponse, filterChain);
    }

    public void doFilter(HttpServletRequest request, HttpServletResponse response, FilterChain chain)
            throws ServletException, IOException {
        final String path = request.getServletPath();
        final boolean isApi = path.startsWith("/api");
        final boolean isResourceFile = !isApi && FILE_EXTENSIONS.stream().anyMatch(path::contains);
        if (isApi) {
            chain.doFilter(request, response);
        } else if (isResourceFile) {
            resourceToResponse("static" + path, response);
        } else {
            resourceToResponse("static/index.html", response);
        }
    }

    private void resourceToResponse(final String resourcePath, final HttpServletResponse response) throws IOException {
        try (final InputStream inputStream = Thread.currentThread()
                .getContextClassLoader()
                .getResourceAsStream(resourcePath)) {
            if (inputStream == null) {
                response.sendError(NOT_FOUND.value(), NOT_FOUND.getReasonPhrase());
                return;
            }
            if (resourcePath.endsWith(".html")) {
                response.setContentType("text/html");
            }
            if (resourcePath.endsWith(".css")) {
                response.setContentType("text/css");
            }
            if (resourcePath.endsWith(".js")) {
                response.setContentType("text/javascript");
            }
            if (resourcePath.endsWith(".png")) {
                response.setContentType("image/png");
            }
            response.setBufferSize(64*1024);
            inputStream.transferTo(response.getOutputStream());
        } catch (IOException e) {
            throw new IOException(e);
        }
    }
}
